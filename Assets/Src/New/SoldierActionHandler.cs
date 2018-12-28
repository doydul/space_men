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

    public bool AnyActionApplicableFor(Soldier soldier, Tile targetTile) {
        if (soldier == null) return false;
        if (gamePhase.movement) {
            return AnyMovingActionApplicableFor(soldier, targetTile);
        } else {
            return AnyShootingActionApplicableFor(soldier, targetTile);
        }
    }

    public void PerformActionFor(Soldier soldier, Tile targetTile) {
        if (soldier == null) return;
        if (gamePhase.movement) {
            PerformMoveActionFor(soldier, targetTile);
        } else {
            PerformShootingActionFor(soldier, targetTile);
        }
    }

    bool AnyMovingActionApplicableFor(Soldier soldier, Tile targetTile) {
        var path = pathingAndLOS.GetPath(soldier.tile, targetTile);
        return targetTile.open && !targetTile.occupied && path.Count <= soldier.remainingMovement;
    }

    bool AnyShootingActionApplicableFor(Soldier soldier, Tile targetTile) {
        var alien = targetTile.GetActor<Alien>();
        return alien != null && soldier.hasAmmo && soldier.WithinSightArc(targetTile.gridLocation) && !pathingAndLOS.LOSBlocked(soldier.tile, targetTile);
    }

    void PerformMoveActionFor(Soldier soldier, Tile targetTile) {
        if (!AnyMovingActionApplicableFor(soldier, targetTile)) return;
        var path = pathingAndLOS.GetPath(soldier.tile, targetTile);
        soldier.MoveTo(targetTile);
        soldier.TurnTo(targetTile.gridLocation - path.Last());
        TriggerTileWalkedOnEvents(path.nodes, targetTile);
        soldierMoved.Invoke();
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
    }
}
