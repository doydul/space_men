using UnityEngine;

public class SoldierLOSWrapper : IBlocker {

    private Map map;

    public SoldierLOSWrapper(Map map) {
        this.map = map;
    }

    public float Blockage(Vector2 gridLocation) {
        var tile = map.GetTileAt(gridLocation);
        if (!tile.open || tile.foggy) return 1;
        if (tile.GetActor<Soldier>() != null) return 0.35f;
        return 0;
    }

    public bool ValidTarget(Vector2 gridLocation) {
        return !map.GetTileAt(gridLocation).foggy;
    }
}