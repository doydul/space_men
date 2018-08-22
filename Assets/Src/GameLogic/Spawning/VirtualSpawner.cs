using UnityEngine;
using System.Collections.Generic;

public class VirtualSpawner {

    public const int SPAWN_DISTANCE = 10;

    private PathFinder pathFinder;
    private ISpawnModule spawnModule;
    private SpawnableLocations spawnableLocations;

    public bool finished { get { return spawnModule == null || spawnModule.finished; } }

    public VirtualSpawner(Map map, ISpawnModule spawnModule, Vector2 gridLocation) {
        var wrapper = new AlienSpawnWrapper(map);
        spawnableLocations = new SpawnableLocations(wrapper);
        pathFinder = new PathFinder(new AlienPathingWrapper(map), gridLocation);
        this.spawnModule = spawnModule;
    }

    public List<VirtualAlien> GetVirtualAliens(List<Vector2> targets) {
        var spawnLocations = SpawnLocations(targets);
        if (spawnLocations.Count <= 0) {
            spawnModule = null;
            return new List<VirtualAlien>();
        }
        var spawnLocation = spawnLocations[Random.Range(0, spawnLocations.Count)];
        int spawnCount = spawnModule.GetVirtualAliensCount();
        var result = new List<VirtualAlien>();
        foreach (var gridLocation in spawnableLocations.GetLocationsNear(spawnLocation, spawnCount)) {
            result.Add(new VirtualAlien(gridLocation, Actor.Direction.Up));
        }
        return result;
    }

    private List<Vector2> SpawnLocations(List<Vector2> targets) {
        var path = pathFinder.FindPath(targets);
        var possibleLocations = new List<Vector2>();
        foreach (var gridLocation in path) {
            if (spawnableLocations.SpawnableLocation(gridLocation)) {
                possibleLocations.Add(gridLocation);
            }
        }
        return new Path(possibleLocations).Last(4);
    }
}
