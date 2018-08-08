using UnityEngine;
using System.Collections.Generic;

public interface ISpawnModule {
    
    bool finished { get; }
    
    List<VirtualAlien> GetVirtualAliens(SpawnableLocations spawnableLocations, Vector2 targetLocation);
}