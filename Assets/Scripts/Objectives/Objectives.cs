using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class Objectives {
    
    public bool allComplete => objectives.Where(objective => objective.required).All(objective => objective.complete);

    public Map map;
    public Map.Room startingRoom;
    public List<Objective> objectives = new();

    Dictionary<int, Dictionary<int, int>> roomDistances = new();
    
    public static void AddToMap(Map map, Map.Room startingRoom) {
        var objectives = new Objectives { map = map, startingRoom = startingRoom };
        objectives.AddObjective(new GetToTarget { required = true });
        objectives.AddObjective(new GrabTheLoot());

        map.objectives = objectives;

        GameEvents.On(objectives, "alien_turn_start", objectives.CheckCompletion);
    }

    void OnDestroy() => GameEvents.RemoveListener(this, "alien_turn_start");

    public void AddObjective(Objective objective) {
        if (!roomDistances.ContainsKey(startingRoom.id)) roomDistances.Add(startingRoom.id, GetDistancesFrom(startingRoom));

        int objectiveRoomId = map.rooms.Values
            .Where(room => !roomDistances.ContainsKey(room.id))
            .MaxBy(room => roomDistances.Keys.ToList().Aggregate(0, (acc, roomId) => acc + roomDistances[roomId][room.id])).id;
        roomDistances.Add(objectiveRoomId, GetDistancesFrom(map.rooms[objectiveRoomId]));
        objective.Init(map.rooms[objectiveRoomId]);
        objectives.Add(objective);
    }
    
    public int EstimateTravelDistance() {
        var objectiveRoomIds = roomDistances.Keys.ToList();
        objectiveRoomIds.Remove(startingRoom.id);
        return EstimateTravelDistanceRecursive(startingRoom.id, objectiveRoomIds);
    }
    int EstimateTravelDistanceRecursive(int previousRoomId, List<int> remainingRoomIds) {
        if (remainingRoomIds.Count == 1) {
            return roomDistances[previousRoomId][remainingRoomIds[0]];
        }
        var results = new List<int>();
        foreach (var roomId in remainingRoomIds) {
            var newList = new List<int>(remainingRoomIds);
            newList.Remove(roomId);
            results.Add(roomDistances[previousRoomId][roomId] + EstimateTravelDistanceRecursive(roomId, newList));
        }
        return results.Min();
    }

    void CheckCompletion() {
        if (allComplete) {
            ModalPopup.instance.DisplayEOL();
        }
    }

    Dictionary<int, int> GetDistancesFrom(Map.Room fromRoom) {
        var roomDistanceMapping = new Dictionary<int, int>();
        var roomCentres = new HashSet<Vector2>();
        foreach (var room in map.rooms.Values) roomCentres.Add(room.centre);

        foreach (var node in map.iterator.Exclude(new ObjectivePathingMask()).EnumerateFrom(fromRoom.centre)) {
            if (roomCentres.Contains(node.tile.gridLocation)) {
                var room = map.rooms.Values.ToList().Find(room => room.centre == node.tile.gridLocation);
                if (room.id == fromRoom.id) continue;
                roomDistanceMapping.Add(room.id, node.distanceFromOrigin);
            }
        }
        return roomDistanceMapping;
    }
}

public class ObjectivePathingMask : IMask {
    public bool Contains(Tile tile) {
        return !tile.open;
    }
}