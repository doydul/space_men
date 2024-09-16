using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class Objectives {
    
    public bool allComplete => objectives.Where(objective => objective.required).All(objective => objective.complete);

    public Map map;
    public Map.Room startingRoom;
    public List<Objective> objectives = new();

    Dictionary<int, Dictionary<int, int>> roomDistances = new();
    
    public static void AddToMap(Map map, Map.Room startingRoom, int equipments) {
        var objectives = new Objectives { map = map, startingRoom = startingRoom };
        new GetToTarget { required = true }.Init(objectives);
        new ActivateTerminal { required = true }.Init(objectives);
        for (int i = 0; i < equipments; i++) {
            new GrabTheLoot().Init(objectives);
        }

        map.objectives = objectives;
        
        ObjectivesPanel.instance.DisplayPrimaryObjectives(objectives.objectives.Where(obj => obj.required).ToList());
        ObjectivesPanel.instance.DisplaySecondaryObjectives(objectives.objectives.Where(obj => !obj.required).ToList());

        GameEvents.On(objectives, "alien_turn_start", objectives.CheckCompletion);
    }

    void OnDestroy() => GameEvents.RemoveListener(this, "alien_turn_start");
    
    public void AddObjective(Map.Room room, Objective objective) {
        if (!roomDistances.ContainsKey(startingRoom.id)) roomDistances.Add(startingRoom.id, GetDistancesFrom(startingRoom));

        roomDistances.Add(room.id, GetDistancesFrom(room));
        objectives.Add(objective);
    }
    
    public Map.Room GetNextBestRoom() {
        if (!roomDistances.ContainsKey(startingRoom.id)) roomDistances.Add(startingRoom.id, GetDistancesFrom(startingRoom));
        
        return map.rooms.Values
            .Where(room => !roomDistances.ContainsKey(room.id))
            .MaxBy(room => roomDistances.Keys.ToList().Aggregate(0, (acc, roomId) => acc + roomDistances[roomId][room.id]));
    }
    
    public Map.Room GetUnoccupiedRoomWithIdealDistance(Map.Room otherRoom, int idealDistance) {
        if (!roomDistances.ContainsKey(startingRoom.id)) roomDistances.Add(startingRoom.id, GetDistancesFrom(startingRoom));
        
        return map.rooms.Values
            .Where(room => !roomDistances.ContainsKey(room.id))
            .MinBy(room => Mathf.Abs(roomDistances[otherRoom.id][room.id] - idealDistance));
    }
    
    public List<Map.Room> GetUnoccupiedRooms() {
        return map.rooms.Values
            .Where(room => !roomDistances.ContainsKey(room.id)).ToList();
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
            Mission.current.End();
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