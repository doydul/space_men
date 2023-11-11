using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
                var adjacentTiles = Map.instance.AdjacentTiles(target.tile, true).Where(tile => tile.open).ToArray();
                for (int j = 0; j < 2; j++) {
                    var randTile = adjacentTiles[Random.Range(0, adjacentTiles.Length)];
                    var actor = randTile.GetActor<Actor>();
                    if (actor != null && actor != soldier) {
                        var damage = Random.Range(soldier.minDamage, soldier.maxDamage + 1);
                        actor.Hurt(damage);
                        actor.ShowHit();
                        BloodSplatController.instance.MakeSplat(actor);
                        break;
                    }
                }
            }
            soldier.HideMuzzleFlash();
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
            FogManager.instance.UpdateFog();
        }
    }

    public static IEnumerator PerformOpenDoor(Soldier soldier, Tile tile) {
        tile.GetBackgroundActor<Door>().Remove();
        yield return new WaitForSeconds(1f);
        FogManager.instance.UpdateFog();
        soldier.HighlightActions();
    }

    public static IEnumerator PerformExplosion(Weapon weapon, Tile tile) {
        yield return new WaitForSeconds(0.5f);
        float remainingBlast = weapon.blast;
        var explosionSFX = new List<GameObject>();
        int iLayer = 0;
        foreach (var layer in Map.instance.iterator.Exclude(new ExplosionImpassableTerrain()).EnumerateLayersFrom(tile.gridLocation)) {
            remainingBlast -= iLayer * 2;
            iLayer++;
            while (layer.Count() > 0 && remainingBlast > 0) {
                remainingBlast -= 1;
                var randex = Random.Range(0, layer.Count());
                var randTile = layer[randex];
                layer.RemoveAt(randex);

                explosionSFX.Add(SFXLayer.instance.SpawnExplosion(randTile.realLocation));
                var actor = randTile.GetActor<Actor>();
                if (actor != null) {
                    var damage = Random.Range(weapon.minDamage, weapon.maxDamage + 1);
                    actor.Hurt(damage);
                    BloodSplatController.instance.MakeSplat(actor);
                }
            }
        }
        yield return new WaitForSeconds(0.5f);
        foreach (var sfx in explosionSFX) Tile.Destroy(sfx);
    }
}

public class ExplosionImpassableTerrain : IMask {
    public bool Contains(Tile tile) {
        return !tile.open || tile.GetBackgroundActor<Door>() != null;
    }
}