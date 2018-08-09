using UnityEngine;
using System.Collections.Generic;

public class VirtualSpawner {

    public const int SPAWN_DISTANCE = 10;

    private PathFinder pathFinder;
    private ISpawnModule spawnModule;
    private SpawnableLocations spawnableLocations;

    public bool finished { get { return spawnModule.finished; } }

    public VirtualSpawner(Map map, ISpawnModule spawnModule, Vector2 gridLocation) {
        var wrapper = new AlienSpawnWrapper(map);
        spawnableLocations = new SpawnableLocations(wrapper);
        pathFinder = new PathFinder(new AlienPathingWrapper(map), gridLocation);
        this.spawnModule = spawnModule;
    }

    public List<VirtualAlien> GetVirtualAliens(List<Vector2> targets) {
        var path = new Path(pathFinder.FindPath(targets));
        return spawnModule.GetVirtualAliens(spawnableLocations, path.NthFromEnd(SPAWN_DISTANCE));
    }
}
