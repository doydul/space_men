using UnityEngine;
using System.Collections.Generic;

public class SoldierPathingAndLOS : SoldierActionHandler.IPathingAndLOS, IBlocker, IPathable {

    Map map;

    public SoldierPathingAndLOS(Map map) {
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

    public bool LocationPathable(Vector2 gridLocation) {
        var tile = map.GetTileAt(gridLocation);
        if (tile.actor != null && tile.GetActor<Soldier>() == null) return false;
        return tile.open;
    }

    //

    public bool LOSBlocked(Tile startTile, Tile endTile) {
        return new LineOfSight(startTile.gridLocation, endTile.gridLocation, this).Blocked();
    }

    public Path GetPath(Tile startTile, Tile endTile) {
        var targets = new List<Vector2>() { endTile.gridLocation };
        return new Path(new PathFinder(this, startTile.gridLocation, targets).FindPath());
    }

    public Tile GetTileAt(Vector2 gridLocation) {
        return map.GetTileAt(gridLocation);
    }
}
