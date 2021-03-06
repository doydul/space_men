using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SoldierPathingAndLOS : SoldierActionHandler.IPathingAndLOS, IBlocker, IPathable, IIterableGrid {

    Map map;

    public SoldierPathingAndLOS(Map map) {
        this.map = map;
    }

    public float Blockage(Vector2 gridLocation) {
        var tile = GetTileAt(gridLocation);
        if (!tile.open || tile.foggy) return 1;
        if (tile.GetActor<Soldier>() != null) return 0.35f;
        return 0;
    }

    public bool ValidTarget(Vector2 gridLocation) {
        return !GetTileAt(gridLocation).foggy;
    }

    public bool LocationPathable(Vector2 gridLocation) {
        var tile = GetTileAt(gridLocation);
        if (tile.actor != null && tile.GetActor<Soldier>() == null) return false;
        // foreach (var square in new AdjacentSquaresGridIterator(this).Squares(gridLocation)) {
        //     var adjTile = GetTileAt(square);
        //     if (tile.GetActor<Alien>() != null) return false;
        // }
        return tile.open;
    }
    
    public bool ShouldIterate(Vector2 gridLocation) {
        var tile = GetTileAt(gridLocation);
        return tile != null && tile.open;
    }

    //

    public bool LOSBlocked(Tile startTile, Tile endTile) {
        return new LineOfSight(startTile.gridLocation, endTile.gridLocation, this).Blocked();
    }

    public Path GetPath(Tile startTile, Tile endTile) {
        var wrapper = new SoldierPathingWrapper2(map, endTile.gridLocation);
        return new Path(new PathFinder2(wrapper).ShortestPath(startTile.gridLocation).path.ToList());
    }

    public Tile GetTileAt(Vector2 gridLocation) {
        return map.GetTileAt(gridLocation);
    }
    
    public bool ValidMoveLocation(Vector2 gridLocation) {
        var tile = GetTileAt(gridLocation);
        Debug.Log(gridLocation);
        Debug.Log(LocationPathable(gridLocation) && tile != null && !tile.occupied);
        return  LocationPathable(gridLocation) && tile != null && !tile.occupied;
    }
}
