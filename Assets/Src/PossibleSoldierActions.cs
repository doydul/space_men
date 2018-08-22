using UnityEngine;
using System.Collections.Generic;

public class PossibleSoldierActions {

    private Map map;
    private Soldier soldier;

    public PossibleSoldierActions(Map map, Soldier soldier) {
        this.map = map;
        this.soldier = soldier;
    }

    public bool Any() {
        if (!soldier.hasAmmo) return false;

        foreach (var alien in map.GetActors<Alien>()) {
            var los = new LineOfSight(soldier.gridLocation, alien.gridLocation, new SoldierLOSWrapper(map));
            if (!alien.dead && soldier.WithinSightArc(alien.gridLocation) && !los.Blocked()) {
                return true;
            }
        }
        return false;
    }
}
