using UnityEngine;

public class SoldierPathingWrapper : IPathable {
    
    private Map map;
    
    public SoldierPathingWrapper(Map map) {
        this.map = map;
    }
    
    public bool LocationPathable(Vector2 gridLocation) {
        var tile = map.GetTileAt(gridLocation);
        if (tile.actor != null && tile.GetActor<Soldier>() == null) return false;
        return tile.open;
    }
}