using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGenerator {

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

        public void Add(Element element) => elements.Add(element);
        public void Add(Element element, Port port, Element parent, Port parentPort) {
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
        }

        public IEnumerable<Element> GetElements() => elements;

        public void Imprint(MapLayout layout) {
            foreach (var element in elements) element.Imprint(layout);
        }
    }
    
    public MapLayout Generate() {
        var walls = new MapLayout();
        var elements = new ElementMap();
        var firstRoom = new Room {
            template = Resources.Load<RoomTemplate>("SpecialRooms/StartingRoom"),
            facing = Facings.Sample()
        };
        elements.Add(firstRoom);
        Element lastEl = firstRoom;
        for (int i = 0; i < 6; i++) {
            var port = lastEl.unnocupiedPorts.Sample();
            var corridor = new Corridor {
                length = Random.Range(4, 6),
                direction = port.outgoingDirections.Sample()
            };
            elements.Add(corridor, corridor.ports[0], lastEl, port);
            var room = new Room {
                template = RoomTemplate.Random(),
                facing = Facings.Sample()
            };
            var ports = room.ports.Where(port => port.incomingDirections.Contains(corridor.direction));
            while (!ports.Any()) {
                room.facing = room.facing.RotateBy(Facing.East);
                ports = room.ports.Where(port => port.incomingDirections.Contains(corridor.direction));
            }
            elements.Add(room, ports.Sample(), corridor, corridor.ports[1]);
            lastEl = room;
        }

        // secondary rooms
        for (int i = 0; i < 5; i++) {
            var element = elements.GetElements().Where(el => el.unnocupiedPorts.Any()).Sample();
            var port = element.unnocupiedPorts.Sample();
            var corridor = new Corridor {
                length = Random.Range(5, 10),
                direction = port.outgoingDirections.Sample()
            };
            elements.Add(corridor, corridor.ports[0], element, port);

            var room = new Room {
                template = RoomTemplate.Random(),
                facing = Facings.Sample()
            };
            var ports = room.ports.Where(port => port.incomingDirections.Contains(corridor.direction));
            while (!ports.Any()) {
                room.facing = room.facing.RotateBy(Facing.East);
                ports = room.ports.Where(port => port.incomingDirections.Contains(corridor.direction));
            }
            elements.Add(room, ports.Sample(), corridor, corridor.ports[1]);
        }

        // vents
        for (int i = 0; i < 5; i++) {
            var element = elements.GetElements().Where(el => el.unnocupiedPorts.Any()).Sample();
            var port = element.unnocupiedPorts.Sample();
            var corridor = new Corridor {
                length = Random.Range(2, 4),
                direction = port.outgoingDirections.Sample()
            };
            elements.Add(corridor, corridor.ports[0], element, port);

            var room = new Room { template = Resources.Load<RoomTemplate>("SpecialRooms/Vent") };
            elements.Add(room, room.ports[0], corridor, corridor.ports[1]);
        }

        elements.Imprint(walls);
        return walls;
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
    public override string ToString() => $"Point({x}, {y})";

    public MapPoint Mirror(bool mirror = true) => mirror ? new MapPoint(-x, y) : this;
}

public class MapLayout {

    public class Tile {
        public MapPoint point;
        public bool isWall;
        public bool isAlienSpawner;
        public bool isPlayerSpawner;
        public bool isLootSpawner;
    }

    public List<List<Tile>> tiles => CalculateTiles();

    List<Tile> openTiles = new();

    public void AddOpenTile(MapPoint point, bool isAlienSpawner, bool isPlayerSpawner, bool isLootSpawner) => openTiles.Add(new Tile { point = point, isWall = false, isAlienSpawner = isAlienSpawner, isPlayerSpawner = isPlayerSpawner, isLootSpawner = isLootSpawner });

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