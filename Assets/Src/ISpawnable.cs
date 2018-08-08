using UnityEngine;

public interface ISpawnable {
    
    bool SpawnableLocation(Vector2 gridLocation);
    
    bool WallLocation(Vector2 gridLocation);
}