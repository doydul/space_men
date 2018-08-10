using UnityEngine;
using System.Collections.Generic;

public class SingleSpawnModule : ISpawnModule {
    
    private bool expended;
    
    public bool finished { get { return expended; } }
    
    public List<VirtualAlien> GetVirtualAliens(SpawnableLocations spawnableLocations, Vector2 targetLocation) {
        var result = new List<VirtualAlien>();
        foreach (var gridLocation in spawnableLocations.GetLocationsNear(targetLocation, 1)) {
            result.Add(new VirtualAlien(gridLocation));
        }
        expended = true;
        return result;
    }
}