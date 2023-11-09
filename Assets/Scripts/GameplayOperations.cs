using UnityEngine;
using System.Collections;
using System.Linq;

public static class GameplayOperations {
    
    public static IEnumerator PerformSoldierShoot(Soldier soldier, Alien target) {
        soldier.Face(target.gridLocation);
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < soldier.shots; i++) {
            soldier.ShowMuzzleFlash();
            yield return new WaitForSeconds(0.2f);
            if (!target.dead && Random.value * 100 <= soldier.accuracy) {
                // HIT
                target.ShowHit();
                var damage = Random.Range(soldier.minDamage, soldier.maxDamage + 1);
                target.Hurt(damage);
                BloodSplatController.instance.MakeSplat(target);
            } else {
                // MISS
                var adjacentTiles = Map.instance.AdjacentTiles(target.tile, true).ToArray();
                for (int j = 0; j < 2; j++) {
                    var randTile = adjacentTiles[Random.Range(0, adjacentTiles.Length)];
                    var actor = randTile.GetActor<Actor>();
                    if (actor != null) {
                        var damage = Random.Range(soldier.minDamage, soldier.maxDamage + 1);
                        actor.Hurt(damage);
                        BloodSplatController.instance.MakeSplat(actor);
                        break;
                    }
                }
            }
            soldier.HideMuzzleFlash();
            target.HideHit();
            yield return new WaitForSeconds(0.2f);
        }
        soldier.HighlightActions();
    }

    public static IEnumerator PerformActorMove(Actor actor, Vector2 gridLocation) {
        // this will be a proper animation at some point
        yield return null;
        actor.MoveTo(Map.instance.GetTileAt(gridLocation));
        if (actor is Soldier) {
            foreach (var alien in Map.instance.GetActors<Alien>()) {
                if (Map.instance.ManhattanDistance(actor.gridLocation, alien.gridLocation) <= alien.sensoryRange) {
                    alien.Awaken();
                }
            }
        }
        if (actor is Soldier) {
            (actor as Soldier).HighlightActions();
        }
    }
}