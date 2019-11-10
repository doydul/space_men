using UnityEngine;
using System.Collections.Generic;

public class SoldierActionHandler {

    public SoldierActionHandler(IPathingAndLOS pathingAndLOS, GamePhase gamePhase, IGameEvent soldierMoved) {
        this.pathingAndLOS = pathingAndLOS;
        this.gamePhase = gamePhase;
        this.soldierMoved = soldierMoved;
    }

    IPathingAndLOS pathingAndLOS;
    GamePhase gamePhase;
    IGameEvent soldierMoved;

    public void PerformActionFor(Soldier soldier, Tile targetTile) {
        if (soldier == null) return;
        if (gamePhase.movement) {
            PerformMoveActionFor(soldier, targetTile);
        } else {
            PerformShootingActionFor(soldier, targetTile);
        }
    }

    bool AnyShootingActionApplicableFor(Soldier soldier, Tile targetTile) {
        var alien = targetTile.GetActor<Alien>();
        return alien != null && soldier.hasAmmo && soldier.WithinSightArc(targetTile.gridLocation) && !pathingAndLOS.LOSBlocked(soldier.tile, targetTile);
    }

    void PerformMoveActionFor(Soldier soldier, Tile targetTile) {
        MapController.instance.MoveSoldier(soldier.index, targetTile.gridLocation);
    }

    void PerformShootingActionFor(Soldier soldier, Tile targetTile) {
        if (!AnyShootingActionApplicableFor(soldier, targetTile)) return;
        var alien = targetTile.GetActor<Alien>();
        GameActions.Shoot(soldier, alien);
    }

    void TriggerTileWalkedOnEvents(List<Vector2> path, Tile target) {
        for (int i = 1; i < path.Count; i++) {
            pathingAndLOS.GetTileAt(path[i]).SoldierEnter.Invoke();
        }
        target.SoldierEnter.Invoke();
    }

    public interface IPathingAndLOS {

        bool LOSBlocked(Tile startTile, Tile endTile);

        Path GetPath(Tile startTile, Tile endTile);

        Tile GetTileAt(Vector2 gridLocation);
        
        bool ValidMoveLocation(Vector2 gridLocation);
    }
}
