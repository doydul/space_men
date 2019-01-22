using UnityEngine;
using System.Collections.Generic;

public class AlienPathingWrapperI : IAlienGrid {

    Map world;

    public AlienPathingWrapperI(Map world) {
        this.world = world;
    }

    public bool ShouldIterate(Vector2 gridLocation) {
        var tile = world.GetTileAt(gridLocation);
        if (tile == null || tile.actor != null && tile.GetActor<Alien>() == null) return false;
        return tile.open;
    }

    public bool IsTargetLocation(Vector2 gridLocation) {
        foreach (var soldier in world.GetActors<Soldier>()) {
            int distance = (int)Mathf.Round(Mathf.Abs(soldier.gridLocation.x - gridLocation.x) + Mathf.Abs(soldier.gridLocation.y - gridLocation.y));
            if (distance == 1) return true;
        }
        return false;
    }

    public bool IsValidFinishLocation(Vector2 gridLocation) {
        return world.GetTileAt(gridLocation).GetActor<Alien>() == null;
    }
}
