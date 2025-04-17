using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGenerator {
    
    [System.Serializable]
    public class Blueprint {
        public int seed;
        public int corridors;
        public int secondaryCorridors;
        public int loops;
        public int rooms;
        public List<Objective> objectives;
    }
    
    [System.Serializable]
    public enum Facing {
        North,
        East,
        South,
        West
    }

    static Facing[] Facings = { Facing.North, Facing.South, Facing.West, Facing.East };
    
    public class Port {
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
    
    class Room {
        public static int currentId;
        public int id;
        public RoomTemplate template;
        public Facing facing;
        public MapPoint centre;

        public Room() {
            currentId++;
            id = currentId;
        }

        public Port[] GetPorts() => template.GetPorts(facing, false);
        public void Imprint(MapLayout layout) => template.Imprint(layout, centre, facing, false, id);
    }
    
    class Corridor {
        public int length;
        public Facing direction;
        public MapPoint start;

        public void Imprint(MapLayout layout) {
            for (int i = 0; i < length; i++) {
                layout.AddOpenTile(start + direction.ToVector() * i);
            }
        }
    }
    
    class Connection {
        public bool doubleWidth;
    }
    
    class Connections {
        
        Dictionary<string, Connection> connections = new();
        
        string ConnectionString(MapPoint point1, MapPoint point2) {
            if (point1.manhattanDistance < point2.manhattanDistance) return point1.ToString() + point2.ToString();
            else return point2.ToString() + point1.ToString();
        }
        
        public void Add(MapPoint point1, MapPoint point2, Connection connection = null) => connections[ConnectionString(point1, point2)] = connection ?? new Connection();
        public bool Connected(MapPoint point1, MapPoint point2) => connections.ContainsKey(ConnectionString(point1, point2));
        public Connection Get(MapPoint point1, MapPoint point2) => connections[ConnectionString(point1, point2)];
    }
    
    Blueprint blueprint;
    
    public MapGenerator(Blueprint blueprint) {
        this.blueprint = blueprint;
    }

    public MapLayout Generate() {
        Random.InitState(blueprint.seed);
        Room.currentId = 0;
        var startPoint = new MapPoint(0, 0);
        var connections = new Connections();
        var graphNodes = new List<MapPoint>();
        graphNodes.Add(startPoint);
        var graphNodesSet = new HashSet<MapPoint>();
        graphNodesSet.Add(startPoint);
        var ports = new List<Port>();
        
        // create graph
        for (int i = 0; i < blueprint.corridors; i++) {
            var currentNode = graphNodes.Sample();
            
            var nextNode = AdjacentNodes(currentNode).Where(node => !graphNodesSet.Contains(node) && !connections.Connected(currentNode, node)).Sample();
            
            graphNodes.Add(nextNode);
            graphNodesSet.Add(nextNode);
            connections.Add(currentNode, nextNode, new Connection { doubleWidth = Random.value < 0.2f });
        }
        
        // add loops
        var possibleLoops = new List<MapPoint[]>();
        foreach (var node in graphNodes) {
            var loopableNodes = AdjacentNodes(node).Where(n => graphNodesSet.Contains(n) && !connections.Connected(node, n) && node.manhattanDistance > n.manhattanDistance).Select(n => new MapPoint[] { node, n }).ToList();
            possibleLoops.AddRange(loopableNodes);
        }
        foreach (var pair in possibleLoops.Sample(blueprint.loops)) {
            connections.Add(pair[0], pair[1]);
        }
        
        // render graph
        var layout = new MapLayout();
        foreach (var node in graphNodes) {
            var realNode = node * 7;
            layout.AddOpenTile(realNode);
            foreach (var adjNode in AdjacentNodes(node)) {
                if (connections.Connected(node, adjNode) && node.manhattanDistance > adjNode.manhattanDistance) {
                    var connection = connections.Get(node, adjNode);
                    var realAdjNode = adjNode * 7;
                    if (node.x == adjNode.x) {
                        var start = System.Math.Min(realNode.y, realAdjNode.y);
                        var end = System.Math.Max(realNode.y, realAdjNode.y);
                        for (int i = start + 1; i < end; i++) {
                            var doorFacing = Door.Facing.None;
                            if (connection.doubleWidth) layout.AddOpenTile(new MapPoint(realNode.x + 1, i));
                            if (Random.value < 0.33f) {
                                ports.Add(new Port {
                                    relativePosition = new MapPoint(realNode.x, i),
                                    direction = Random.value < 0.5f ? Facing.West : Facing.East
                                });
                                if (connection.doubleWidth) {
                                    ports.Add(new Port {
                                        relativePosition = new MapPoint(realNode.x + 1, i),
                                        direction = Facing.East
                                    });
                                }
                            } else if (i > start + 1 && i < end - 1 && !connection.doubleWidth && Random.value < 0.1f) {
                                doorFacing = Door.Facing.NorthSouth;
                            }
                            layout.AddOpenTile(new MapPoint(realNode.x, i), false, false, false, -1, doorFacing);
                        }
                    } else {
                        var start = System.Math.Min(realNode.x, realAdjNode.x);
                        var end = System.Math.Max(realNode.x, realAdjNode.x);
                        for (int i = start + 1; i < end; i++) {
                            var doorFacing = Door.Facing.None;
                            if (connection.doubleWidth) layout.AddOpenTile(new MapPoint(i, realNode.y + 1));
                            if (Random.value < 0.33f) {
                                ports.Add(new Port {
                                    relativePosition = new MapPoint(i, realNode.y),
                                    direction = Random.value < 0.5f ? Facing.North : Facing.South
                                });
                                if (connection.doubleWidth) {
                                    ports.Add(new Port {
                                        relativePosition = new MapPoint(i, realNode.y + 1),
                                        direction = Facing.North
                                    });
                                }
                            } else if (i > start + 1 && i < end - 1 && !connection.doubleWidth && Random.value < 0.1f) {
                                doorFacing = Door.Facing.EastWest;
                            }
                            layout.AddOpenTile(new MapPoint(i, realNode.y), false, false, false, -1, doorFacing);
                        }
                    }
                }
            }
        }
        
        // add special rooms
        foreach (var objective in blueprint.objectives) {
            foreach (var roomTemplate in objective.specialRooms) {
                for (int i = 0 ; i < 50; i++) {
                    var roomId = AddRoom(ports, layout, roomTemplate);
                    if (roomId != -1) {
                        objective.roomId = roomId;
                        blueprint.rooms--;
                        break;
                    }
                }
            }
        }
        
        // add normal rooms
        int remainingAttempts = 100;
        for (int i = 0; i < blueprint.rooms && remainingAttempts > 0; i++) {
            remainingAttempts--;
            if (AddRoom(ports, layout, RoomTemplate.RandomRoom()) == -1) i--;
        }
        
        // add random corridors
        remainingAttempts = 50;
        for (int i = 0; i < blueprint.secondaryCorridors && remainingAttempts > 0; i++) {
            remainingAttempts--;
            
            var port = ports.Sample();
            if (Random.value < 0.5f) port.direction = port.direction.Opposite();
            var corridor = new Corridor {
                length = Random.Range(2, 4),
                direction = port.direction,
                start = port.relativePosition + port.direction.ToVector() * 2
            };
            
            var newLayout = new MapLayout();
            corridor.Imprint(newLayout);
            if (layout.Overlaps(newLayout)) {
                i--;
            } else {
                corridor.Imprint(layout);
                layout.AddOpenTile(port.relativePosition + port.direction.ToVector());
                ports.Remove(port);
            }
        }
        
        return layout;
    }
    
    int AddRoom(List<Port> ports, MapLayout layout, RoomTemplate template) {
        var port = ports.Sample();
        if (Random.value < 0.5f) port.direction = port.direction.Opposite();
        var room = new Room {
            template = template,
            facing = Facings.Sample()
        };
        var availablePorts = room.GetPorts().Where(p => p.direction == port.direction.Opposite());
        if (!availablePorts.Any()) {
            return -1;
        }
        var roomPort = availablePorts.Sample();
        room.centre = port.relativePosition + port.direction.ToVector() * 2 - roomPort.relativePosition;
        
        var newLayout = new MapLayout();
        room.Imprint(newLayout);
        if (layout.Overlaps(newLayout)) {
            return -1;
        } else {
            room.Imprint(layout);
            var doorFacing = Door.Facing.None;
            if (Random.value < 0.4f) {
                if (port.direction == Facing.North || port.direction == Facing.South) doorFacing = Door.Facing.NorthSouth;
                else doorFacing = Door.Facing.EastWest;
            }
            layout.AddOpenTile(port.relativePosition + port.direction.ToVector(), false, false, false, -1, doorFacing);
            ports.Remove(port);
        }
        return room.id;
    }
    
    IEnumerable<MapPoint> AdjacentNodes(MapPoint point) {
        yield return new MapPoint(point.x, point.y + 1);
        yield return new MapPoint(point.x + 1, point.y);
        yield return new MapPoint(point.x, point.y - 1);
        yield return new MapPoint(point.x - 1, point.y);
    } 
}
