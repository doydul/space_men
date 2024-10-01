using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class Objectives {
    
    public bool allComplete => objectives.Where(objective => objective.required).All(objective => objective.complete);

    public Map map;
    public Map.Room startingRoom;
    public List<Objective> objectives = new();
    public List<Objective> objectivesTmp;

    Dictionary<int, Dictionary<int, int>> roomDistances = new();
    
    public static List<Objective> GenerateObjectiveList(PlayerSave currentSave) {
        var result = new List<Objective> { new GetToTarget { required = true } };
        if (currentSave.levelNumber > 0 && currentSave.levelNumber % 2 == 0) {
            if (Random.value < 0.5f) result.Add(new WaveDefence { required = true });
            else result.Add(new ActivateTerminal { required = true });
        }
        result.Add(new GrabTheLoot());
        result.Add(new GrabTheLoot());
        return result;
    }
    
    public static void AddToMap(Map map, List<Objective> objectiveList, Map.Room startingRoom) {
        var objectives = new Objectives { map = map, startingRoom = startingRoom, objectivesTmp = objectiveList };
        foreach (var objective in objectiveList) {
            objective.Init(objectives);
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
            .Where(room => !roomDistances.ContainsKey(room.id) && !objectivesTmp.Any(obj => obj.roomId == room.id))
            .MaxBy(room => roomDistances.Keys.ToList().Aggregate(0, (acc, roomId) => acc + roomDistances[roomId][room.id]));
    }
    
    public Map.Room GetUnoccupiedRoomWithIdealDistance(Map.Room otherRoom, int idealDistance) {
        if (!roomDistances.ContainsKey(startingRoom.id)) roomDistances.Add(startingRoom.id, GetDistancesFrom(startingRoom));
        
        return map.rooms.Values
            .Where(room => !roomDistances.ContainsKey(room.id) && !objectivesTmp.Any(obj => obj.roomId == room.id))
            .MinBy(room => Mathf.Abs(roomDistances[otherRoom.id][room.id] - idealDistance));
    }
    
    public List<Map.Room> GetUnoccupiedRooms() {
        return map.rooms.Values
            .Where(room => !roomDistances.ContainsKey(room.id) && !objectivesTmp.Any(obj => obj.roomId == room.id))
            .ToList();
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