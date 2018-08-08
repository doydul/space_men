using UnityEngine;

public class AlienSpawnWrapper : ISpawnable {
    
    private Map map;
    
    public AlienSpawnWrapper(Map map) {
        this.map = map;
    }
    
    public bool SpawnableLocation(Vector2 gridLocation) {
        return !WallLocation(gridLocation) && map.GetActorAt<Actor>(gridLocation) == null;
    }
    
    public bool WallLocation(Vector2 gridLocation) {
        var tile = map.GetTileAt(gridLocation);
        return tile == null || !tile.open;
    }
}