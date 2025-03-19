using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using static MapGenerator;

public class RoomTemplate : MonoBehaviour {
    
    public bool isCorridor;
    
    private RoomTemplateTile[] tiles => transform.GetComponentsInChildren<RoomTemplateTile>();

    public Port[] GetPorts(Facing facing, bool mirrored) {
        var result = new List<Port>();
        int i = 0;
        foreach (var tile in tiles) {
            if (tile.isPort) {
                result.Add(new Port {
                    relativePosition = (tile.point * facing).Mirror(mirrored),
                    direction = tile.portDirection.RotateBy(facing),
                    omniDirectional = false,
                    index = i
                });
                i++;
            }
        }
        return result.ToArray();
    }

    public void Imprint(MapLayout layout, MapPoint centre, Facing facing, bool mirrored, int roomId) {
        foreach (var tile in tiles) {
            layout.AddOpenTile((tile.point * facing).Mirror(mirrored) + centre, tile.isAlienSpawner, tile.isPlayerSpawner, tile.isLootSpawner, roomId, Door.Facing.None, tile.ignoreOverlap);
        }
    }

    public static RoomTemplate Random() {
        return Resources.LoadAll<RoomTemplate>("Rooms").Sample();
    }
    
    public static RoomTemplate RandomCorridor() {
        return Resources.LoadAll<RoomTemplate>("Rooms").Where(room => room.isCorridor).Sample();
    }
    
    public static RoomTemplate RandomRoom() {
        return Resources.LoadAll<RoomTemplate>("Rooms").Where(room => !room.isCorridor).Sample();
    }
}