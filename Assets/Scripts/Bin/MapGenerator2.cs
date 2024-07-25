// using System.Collections.Generic;
// using System.Linq;
// using UnityEngine;

// public class MapGenerator {

//     const int MAX_ATTEMPTS = 200;

//     [System.Serializable]
//     public enum Facing {
//         North,
//         East,
//         South,
//         West
//     }

//     static Facing[] Facings = { Facing.North, Facing.South, Facing.West, Facing.East };

//     public struct Port {
//         public MapPoint relativePosition;
//         public Facing direction;
//         public bool omniDirectional;
//         public int index;

//         public Facing[] outgoingDirections { get {
//             var directionTmp = direction;
//             return omniDirectional ? Facings.Where(fac => fac.Opposite() != directionTmp).ToArray() : new Facing[] { directionTmp };
//         } }
//         public Facing[] incomingDirections { get {
//             var directionTmp = direction.Opposite();
//             return omniDirectional ? Facings.Where(fac => fac.Opposite() != directionTmp).ToArray() : new Facing[] { directionTmp };
//         } }
//     }

//     class Connection {
//         public bool parent;
//         public int myPortIndex;
//         public int theirPortIndex;
//         public Element element;
//     }

    // abstract class Element {
    //     public Connection parentConnection => connections.FirstOrDefault(con => con.parent);
    //     public MapPoint parentRelativePosition => parentConnection == null ? new MapPoint(0, 0) : parent.ports.First(port => port.index == parentConnection.theirPortIndex).relativePosition;
    //     public Element parent => parentConnection?.element;
    //     public Element[] children => connections.Where(con => !con.parent).Select(con => con.element).ToArray();
    //     public Port[] ports => GetPorts();
    //     public Port[] unnocupiedPorts => ports.Where(port => !Occupied(port)).ToArray();
    //     public MapPoint centre => parent == null ? new MapPoint(0, 0) : parent.centre + parentRelativePosition - ports.First(ports => ports.index == parentConnection.myPortIndex).relativePosition;

    //     public bool Occupied(Port port) => connections.Any(con => con.myPortIndex == port.index);

    //     public List<Connection> connections = new();

    //     public abstract void Imprint(MapLayout layout);
    //     protected abstract Port[] GetPorts();
    // }

    // class Corridor : Element {
    //     public int length;
    //     public Facing direction;

    //     protected override Port[] GetPorts() {
    //         return new Port[] {
    //             new Port() {
    //                 relativePosition = new MapPoint(0, 0),
    //                 direction = direction.Opposite(),
    //                 omniDirectional = true,
    //                 index = 0
    //             },
    //             new Port() {
    //                 relativePosition = direction.ToVector() * length,
    //                 direction = direction,
    //                 omniDirectional = true,
    //                 index = 1
    //             }
    //         };
    //     }

    //     public override void Imprint(MapLayout layout) {
    //         for (int i = 0; i < length; i++) {
    //             layout.AddOpenTile(centre + direction.ToVector() * i, false, false, false);
    //         }
    //     }
    // }

    // class Room : Element {
    //     static int currentId;
    //     public int id;
    //     public RoomTemplate template;
    //     public Facing facing;

    //     public Room() {
    //         currentId++;
    //         id = currentId;
    //     }

    //     protected override Port[] GetPorts() => template.GetPorts(facing, false);
    //     public override void Imprint(MapLayout layout) => template.Imprint(layout, centre, facing, false, id);
    // }

//     class Vent : Element {
//         RoomTemplate template;

//         public Vent() {
//             template = Resources.Load<RoomTemplate>("SpecialRooms/Vent");
//         }

//         protected override Port[] GetPorts() => template.GetPorts(Facing.West, false);
//         public override void Imprint(MapLayout layout) => template.Imprint(layout, centre, Facing.West, false, -1);
//     }

//     class ElementMap {
//         List<Element> elements = new();
//         public MapLayout mapLayout { get; private set; } = new();

//         public bool CanAdd(Element element, Port port, Element parent, Port parentPort, int maxOverlaps) {
//             var tmpLayout = new MapLayout();
//             var connection = new Connection {
//                 parent = true,
//                 myPortIndex = port.index,
//                 theirPortIndex = parentPort.index,
//                 element = parent
//             };
//             element.connections.Add(connection);
//             element.Imprint(tmpLayout);
//             element.connections.Remove(connection);
//             return !tmpLayout.Overlaps(mapLayout, maxOverlaps);
//         }
//         public void Add(Element element) {
//             elements.Add(element);
//             element.Imprint(mapLayout);
//         }
//         public bool Add(Element element, Port port, Element parent, Port parentPort, int maxOverlaps = 4) {
//             if (!CanAdd(element, port, parent , parentPort, maxOverlaps)) return false;
//             parent.connections.Add(new Connection {
//                 myPortIndex = parentPort.index,
//                 theirPortIndex = port.index,
//                 element = element
//             });
//             element.connections.Add(new Connection {
//                 parent = true,
//                 myPortIndex = port.index,
//                 theirPortIndex = parentPort.index,
//                 element = parent
//             });
//             Add(element);
//             return true;
//         }

//         public IEnumerable<Element> GetElements() => elements;

//         public void Imprint(MapLayout layout) {
//             foreach (var element in elements) element.Imprint(layout);
//         }
//     }
    
//     public MapLayout Generate() {
//         var elements = new ElementMap();
//         var firstRoom = new Room {
//             template = Resources.Load<RoomTemplate>("SpecialRooms/StartingRoom"),
//             facing = Facings.Sample()
//         };
//         elements.Add(firstRoom);
//         Element lastEl = firstRoom;
//         int totalAttempts = 0;
//         int roomCount = 6;
//         int corridorCount = 6;
//         int ventCount = 6;
        
//         // corridors
//         while (corridorCount > 0 && totalAttempts < MAX_ATTEMPTS) {
//             totalAttempts++;
//             var element = elements.GetElements().Where(el => el.unnocupiedPorts.Any()).Sample();
//             var port = element.unnocupiedPorts.Sample();
//             var corridor = new Corridor {
//                 length = Random.Range(3, 5),
//                 direction = port.outgoingDirections.Sample()
//             };
//             if (!elements.Add(corridor, corridor.ports[0], element, port)) continue;

//             var room = new Room {
//                 template = RoomTemplate.RandomCorridor(),
//                 facing = Facings.Sample()
//             };
//             var ports = room.ports.Where(port => port.incomingDirections.Contains(corridor.direction));
//             while (!ports.Any()) {
//                 room.facing = room.facing.RotateBy(Facing.East);
//                 ports = room.ports.Where(port => port.incomingDirections.Contains(corridor.direction));
//             }
//             if (!elements.Add(room, ports.Sample(), corridor, corridor.ports[1])) {
//                 continue;
//             }
//             corridorCount--;
//         }

//         // rooms
//         while (roomCount > 0 && totalAttempts < MAX_ATTEMPTS) {
//             totalAttempts++;
//             var element = elements.GetElements().Where(el => el.unnocupiedPorts.Any()).Sample();
//             var port = element.unnocupiedPorts.Sample();
//             var corridor = new Corridor {
//                 length = Random.Range(3, 5),
//                 direction = port.outgoingDirections.Sample()
//             };
//             if (!elements.Add(corridor, corridor.ports[0], element, port)) continue;

            // var room = new Room {
            //     template = RoomTemplate.RandomRoom(),
            //     facing = Facings.Sample()
            // };
//             var ports = room.ports.Where(port => port.incomingDirections.Contains(corridor.direction));
//             while (!ports.Any()) {
//                 room.facing = room.facing.RotateBy(Facing.East);
//                 ports = room.ports.Where(port => port.incomingDirections.Contains(corridor.direction));
//             }
//             if (!elements.Add(room, ports.Sample(), corridor, corridor.ports[1])) {
//                 continue;
//             }
//             roomCount--;
//         }

//         // vents
//         while (ventCount > 0 && totalAttempts < MAX_ATTEMPTS) {
//             totalAttempts++;
//             var element = elements.GetElements().Where(el => el.unnocupiedPorts.Any()).Sample();
//             if (element == null) break;
            
//             var port = element.unnocupiedPorts.Sample();
//             var corridor = new Corridor {
//                 length = Random.Range(2, 4),
//                 direction = port.outgoingDirections.Sample()
//             };
//             if (!elements.Add(corridor, corridor.ports[0], element, port, 2)) continue;
//             ventCount--;

//             var room = new Vent();
//             elements.Add(room, room.ports[0], corridor, corridor.ports[1]);
//         }

//         return elements.mapLayout;
//     }
// }