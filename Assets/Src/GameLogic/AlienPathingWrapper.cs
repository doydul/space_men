using UnityEngine;

public class AlienPathingWrapper : IPathable {
    
    private Map map;
    
    public AlienPathingWrapper(Map map) {
        this.map = map;
    }
    
    public bool LocationPathable(Vector2 gridLocation) {
        var tile = map.GetTileAt(gridLocation);
        if (tile.actor != null && tile.GetActor<Alien>() == null) return false;
        return tile.open;
    }
}