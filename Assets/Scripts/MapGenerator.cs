using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGenerator {

    const int MAX_ATTEMPTS = 200;

    [System.Serializable]
    public enum Facing {
        North,
        East,
        South,
        West
    }

    static Facing[] Facings = { Facing.North, Facing.South, Facing.West, Facing.East };

    public struct Port {
        public MapPoint relativePosition;
        public Facing direction;
        public bool omniDirectional;
        public int index;

        public Facing[] outgoingDirections { get {
            var directionTmp = direction;
            return omniDirectional ? Facings.Where(fac => fac.Opposite() != directionTmp).ToArray() : new Facing[] { directionTmp };
        } }
        public Facing[] incomingDirections { get {
            var directionTmp = direction.Opposite();
            return omniDirectional ? Facings.Where(fac => fac.Opposite() != directionTmp).ToArray() : new Facing[] { directionTmp };
        } }
    }

    class Connection {
        public bool parent;
        public int myPortIndex;
        public int theirPortIndex;
        public Element element;
    }

    abstract class Element {
        public Connection parentConnection => connections.FirstOrDefault(con => con.parent);
        public MapPoint parentRelativePosition => parentConnection == null ? new MapPoint(0, 0) : parent.ports.First(port => port.index == parentConnection.theirPortIndex).relativePosition;
        public Element parent => parentConnection?.element;
        public Element[] children => connections.Where(con => !con.parent).Select(con => con.element).ToArray();
        public Port[] ports => GetPorts();
        public Port[] unnocupiedPorts => ports.Where(port => !Occupied(port)).ToArray();
        public MapPoint centre => parent == null ? new MapPoint(0, 0) : parent.centre + parentRelativePosition - ports.First(ports => ports.index == parentConnection.myPortIndex).relativePosition;

        public bool Occupied(Port port) => connections.Any(con => con.myPortIndex == port.index);

        public List<Connection> connections = new();

        public abstract void Imprint(MapLayout layout);
        protected abstract Port[] GetPorts();
    }

    class Corridor : Element {
        public int length;
        public Facing direction;

        protected override Port[] GetPorts() {
            return new Port[] {
                new Port() {
                    relativePosition = new MapPoint(0, 0),
                    direction = direction.Opposite(),
                    omniDirectional = true,
                    index = 0
                },
                new Port() {
                    relativePosition = direction.ToVector() * length,
                    direction = direction,
                    omniDirectional = true,
                    index = 1
                }
            };
        }

        public override void Imprint(MapLayout layout) {
            for (int i = 0; i < length; i++) {
                layout.AddOpenTile(centre + direction.ToVector() * i, false, false, false);
            }
        }
    }

    class Room : Element {
        public RoomTemplate template;
        public Facing facing;

        protected override Port[] GetPorts() => template.GetPorts(facing, false);
        public override void Imprint(MapLayout layout) => template.Imprint(layout, centre, facing, false);
    }

    class ElementMap {
        List<Element> elements = new();
        public MapLayout mapLayout { get; private set; } = new();

        public bool CanAdd(Element element, Port port, Element parent, Port parentPort, int maxOverlaps) {
            var tmpLayout = new MapLayout();
            var connection = new Connection {
                parent = true,
                myPortIndex = port.index,
                theirPortIndex = parentPort.index,
                element = parent
            };
            element.connections.Add(connection);
            element.Imprint(tmpLayout);
            element.connections.Remove(connection);
            return !tmpLayout.Overlaps(mapLayout, maxOverlaps);
        }
        public void Add(Element element) {
            elements.Add(element);
            element.Imprint(mapLayout);
        }
        public bool Add(Element element, Port port, Element parent, Port parentPort, int maxOverlaps = 4) {
            if (!CanAdd(element, port, parent , parentPort, maxOverlaps)) return false;
            parent.connections.Add(new Connection {
                myPortIndex = parentPort.index,
                theirPortIndex = port.index,
                element = element
            });
            element.connections.Add(new Connection {
                parent = true,
                myPortIndex = port.index,
                theirPortIndex = parentPort.index,
                element = parent
            });
            Add(element);
            return true;
        }

        public IEnumerable<Element> GetElements() => elements;

        public void Imprint(MapLayout layout) {
            foreach (var element in elements) element.Imprint(layout);
        }
    }
    
    public MapLayout Generate() {
        var elements = new ElementMap();
        var firstRoom = new Room {
            template = Resources.Load<RoomTemplate>("SpecialRooms/StartingRoom"),
            facing = Facings.Sample()
        };
        elements.Add(firstRoom);
        Element lastEl = firstRoom;
        int totalAttempts = 0;
        int roomCount = 6;
        int corridorCount = 12;
        while (roomCount > 0 && totalAttempts < MAX_ATTEMPTS) {
            totalAttempts++;
            var port = lastEl.unnocupiedPorts.Sample();
            var corridor = new Corridor {
                length = Random.Range(4, 6),
                direction = port.outgoingDirections.Sample()
            };
            if (!elements.Add(corridor, corridor.ports[0], lastEl, port)) continue;
            if (corridorCount > 0 && Random.value < 0.5f) {
                var otherCorridor = new Corridor {
                    length = Random.Range(4, 6),
                    direction = corridor.ports[1].outgoingDirections.Sample()
                };
                if (elements.Add(otherCorridor, otherCorridor.ports[0], corridor, corridor.ports[1])) {
                    corridorCount--;
                    corridor = otherCorridor;
                }
            }
            var room = new Room {
                template = RoomTemplate.Random(),
                facing = Facings.Sample()
            };
            var ports = room.ports.Where(port => port.incomingDirections.Contains(corridor.direction));
            while (!ports.Any()) {
                room.facing = room.facing.RotateBy(Facing.East);
                ports = room.ports.Where(port => port.incomingDirections.Contains(corridor.direction));
            }
            if (!elements.Add(room, ports.Sample(), corridor, corridor.ports[1])) {
                corridorCount--;
                continue;
            }
            lastEl = room;
            roomCount--;
        }

        // secondary rooms
        roomCount += 5;
        while ((roomCount > 0 || corridorCount > 0) && totalAttempts < MAX_ATTEMPTS) {
            totalAttempts++;
            if (Random.value < 0.5f) {
                if (roomCount > 0) {
                    var element = elements.GetElements().Where(el => el.unnocupiedPorts.Any()).Sample();
                    var port = element.unnocupiedPorts.Sample();
                    var corridor = new Corridor {
                        length = Random.Range(5, 10),
                        direction = port.outgoingDirections.Sample()
                    };
                    if (!elements.Add(corridor, corridor.ports[0], element, port)) continue;

                    var room = new Room {
                        template = RoomTemplate.Random(),
                        facing = Facings.Sample()
                    };
                    var ports = room.ports.Where(port => port.incomingDirections.Contains(corridor.direction));
                    while (!ports.Any()) {
                        room.facing = room.facing.RotateBy(Facing.East);
                        ports = room.ports.Where(port => port.incomingDirections.Contains(corridor.direction));
                    }
                    if (!elements.Add(room, ports.Sample(), corridor, corridor.ports[1])) {
                        corridorCount--;
                        continue;
                    }
                    roomCount--;
                }
            } else {
                if (corridorCount > 0) {
                    var element = elements.GetElements().Where(el => el.unnocupiedPorts.Any()).Sample();
                    var port = element.unnocupiedPorts.Sample();
                    var corridor = new Corridor {
                        length = Random.Range(5, 10),
                        direction = port.outgoingDirections.Sample()
                    };
                    if (elements.Add(corridor, corridor.ports[0], element, port)) corridorCount--;
                }
            }
        }

        // end and objectives
        // foreach (var element in elements.GetElements().Where(el => el.unnocupiedPorts.Any())) {

        // }

        // vents
        corridorCount += 20;
        while (corridorCount > 0 && totalAttempts < MAX_ATTEMPTS) {
            totalAttempts++;
            var element = elements.GetElements().Where(el => el.unnocupiedPorts.Any()).Sample();
            var port = element.unnocupiedPorts.Sample();
            var corridor = new Corridor {
                length = Random.Range(2, 4),
                direction = port.outgoingDirections.Sample()
            };
            if (!elements.Add(corridor, corridor.ports[0], element, port, 2)) continue;
            corridorCount--;

            var room = new Room { template = Resources.Load<RoomTemplate>("SpecialRooms/Vent") };
            elements.Add(room, room.ports[0], corridor, corridor.ports[1]);
        }

        return elements.mapLayout;
    }
}

public struct MapPoint {
    public int x;
    public int y;
    
    public MapPoint(int x, int y) {
        this.x = x;
        this.y = y;
    }

    public static MapPoint operator *(MapPoint a, int num) => new MapPoint(a.x * num, a.y * num);
    public static MapPoint operator +(MapPoint a, MapPoint b) => new MapPoint(a.x + b.x, a.y + b.y);
    public static MapPoint operator -(MapPoint a, MapPoint b) => new MapPoint(a.x - b.x, a.y - b.y);
    public static MapPoint operator *(MapPoint a, MapGenerator.Facing facing) {
        if (facing == MapGenerator.Facing.South) return new MapPoint(-a.x, -a.y);
        else if (facing == MapGenerator.Facing.East) return new MapPoint(a.y, -a.x);
        else if (facing == MapGenerator.Facing.West) return new MapPoint(-a.y, a.x);
        else return a;
    }
    public bool Equals(MapPoint other) => x == other.x && y == other.y;
    public bool Adjacent(MapPoint other) => Mathf.Abs(x - other.x) + Mathf.Abs(y - other.y) <= 1;
    public override string ToString() => $"Point({x}, {y})";
    public Vector2 ToVec() => new Vector2(x, y);

    public MapPoint Mirror(bool mirror = true) => mirror ? new MapPoint(-x, y) : this;
}

public class MapLayout {

    public class Tile {
        public MapPoint point;
        public bool isWall;
        public bool isAlienSpawner;
        public bool isPlayerSpawner;
        public bool isLootSpawner;
        public bool isSpecial => isAlienSpawner || isPlayerSpawner || isLootSpawner;

        public override string ToString() => $"Tile(point: {point}, isAlienSpawner: {isAlienSpawner}, isPlayerSpawner: {isPlayerSpawner}, isLootSpawner: {isLootSpawner}, isSpecial: {isSpecial})";
    }

    public List<List<Tile>> tiles => CalculateTiles();

    List<Tile> openTiles = new();

    public void AddOpenTile(MapPoint point, bool isAlienSpawner, bool isPlayerSpawner, bool isLootSpawner) {
        if (!openTiles.Any(tile => tile.point.Equals(point))) {
            openTiles.Add(new Tile { point = point, isWall = false, isAlienSpawner = isAlienSpawner, isPlayerSpawner = isPlayerSpawner, isLootSpawner = isLootSpawner });
        }
    }

    public bool Overlaps(MapLayout other, int maxOverlaps = 1) {
        int overlaps = 0;
        var alreadyTraversed = new HashSet<Vector2>();
        foreach (var otherTile in other.openTiles) {
            foreach (var tile in openTiles) {
                if (tile.point.Adjacent(otherTile.point)) {
                    if (tile.point.Equals(otherTile.point) && (tile.isSpecial || otherTile.isSpecial)) {
                        return true;
                    }
                    else if (!alreadyTraversed.Contains(tile.point.ToVec()) && !alreadyTraversed.Contains(otherTile.point.ToVec())) {
                        overlaps++;
                        alreadyTraversed.Add(tile.point.ToVec());
                        alreadyTraversed.Add(otherTile.point.ToVec());
                    }
                }
            }
        }
        return overlaps > maxOverlaps;
    }

    private List<List<Tile>> CalculateTiles() {
        var minX = int.MaxValue;
        var minY = int.MaxValue;
        var maxX = int.MinValue;
        var maxY = int.MinValue;
        foreach (var point in openTiles.Select(tile => tile.point)) {
            if (point.x < minX) minX = point.x;
            if (point.x > maxX) maxX = point.x;
            if (point.y < minY) minY = point.y;
            if (point.y > maxY) maxY = point.y;
        }
        var xMod = -minX + 2;
        var yMod = -minY + 2;
        var width = maxX - minX + 5;
        var height = maxY - minY + 5;
        var result = new List<List<Tile>>();
        for (int x = 0; x < width; x++) {
            result.Add(new List<Tile>());
            for (int y = 0; y < height; y++) {
                result[x].Add(new Tile { point = new MapPoint(x, y), isWall = true});
            }
        }
        foreach (var tile in openTiles) {
            var resultTile = result[tile.point.x + xMod][tile.point.y + yMod];
            resultTile.isWall = false;
            resultTile.isAlienSpawner = tile.isAlienSpawner;
            resultTile.isPlayerSpawner = tile.isPlayerSpawner;
            resultTile.isLootSpawner = tile.isLootSpawner;
        }
        return result;
    }
}

public static class MapLayoutExtenstions {

    public static MapGenerator.Facing Opposite(this MapGenerator.Facing facing) {
        if (facing == MapGenerator.Facing.North) return MapGenerator.Facing.South;
        else if (facing == MapGenerator.Facing.South) return MapGenerator.Facing.North;
        else if (facing == MapGenerator.Facing.West) return MapGenerator.Facing.East;
        else return MapGenerator.Facing.West;
    }

    public static MapPoint ToVector(this MapGenerator.Facing facing) {
        if (facing == MapGenerator.Facing.North) return new MapPoint(0, 1);
        else if (facing == MapGenerator.Facing.South) return new MapPoint(0, -1);
        else if (facing == MapGenerator.Facing.West) return new MapPoint(-1, 0);
        else return new MapPoint(1, 0);
    }

    public static MapGenerator.Facing RotateBy(this MapGenerator.Facing a, MapGenerator.Facing facing) {
        if (facing == MapGenerator.Facing.South) return a.Opposite();
        else if (facing == MapGenerator.Facing.East) return (MapGenerator.Facing)((((int)a)+1) % 4);
        else if (facing == MapGenerator.Facing.West) return (MapGenerator.Facing)((((int)a)+3) % 4);
        else return a;
    }
}