using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class GameplayOperations {

    public static void MakeNoise(Vector2 gridLocation) {
        foreach (var alien in Map.instance.GetActors<Alien>()) {
            if (Map.instance.ManhattanDistance(gridLocation, alien.gridLocation) <= alien.sensoryRange) {
                alien.Awaken();
            }
        }
    }
    
    public static IEnumerator PerformSoldierShoot(Soldier soldier, Alien target) {
        MakeNoise(target.gridLocation);
        soldier.Face(target.gridLocation);
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < soldier.shots; i++) {
            yield return PerformSoldierSingleShot(soldier, target);
        }
        soldier.RefreshUI();
    }

    public static IEnumerator PerformSoldierSingleShot(Soldier soldier, Alien target) {
        soldier.ShowMuzzleFlash();
        soldier.PlayAudio(soldier.weapon.audio.shoot);
        var accuracy = soldier.accuracy;
        if (!soldier.InHalfRange(target.gridLocation)) accuracy -= 15;
        if (!target.dead && Random.value * 100 <= accuracy + target.accModifier) {
            // HIT
            yield return SFXLayer.instance.PerformTracer(soldier.tile.transform.position, target.tile.transform.position, soldier.weapon, true);
            soldier.PlayAudio(soldier.weapon.audio.hit.Sample());
            target.ShowHit();
            var damage = Random.Range(soldier.minDamage, soldier.maxDamage + 1);
            target.Hurt(damage);
            BloodSplatController.instance.MakeSplat(target);
        } else {
            // MISS
            var adjacentTiles = Map.instance.AdjacentTiles(target.tile, true).ToArray();
            bool secondaryHit = false;
            for (int j = 0; j < 2; j++) {
                var randTile = adjacentTiles[Random.Range(0, adjacentTiles.Length)];
                var actor = randTile.GetActor<Actor>();
                if (actor != null && actor != soldier) {
                    secondaryHit = true;
                    yield return SFXLayer.instance.PerformTracer(soldier.tile.transform.position, actor.tile.transform.position, soldier.weapon, true);
                    soldier.PlayAudio(soldier.weapon.audio.hit.Sample());
                    var damage = Random.Range(soldier.minDamage, soldier.maxDamage + 1);
                    actor.Hurt(damage);
                    actor.ShowHit();
                    BloodSplatController.instance.MakeSplat(actor);
                    break;
                }
            }
            if (!secondaryHit) {
                yield return SFXLayer.instance.PerformTracer(soldier.tile.transform.position, target.tile.transform.position, soldier.weapon, false);
                soldier.PlayAudio(soldier.weapon.audio.miss.Sample());
            }
        }
        soldier.HideMuzzleFlash();
        yield return new WaitForSeconds(0.15f);
    }

    public static IEnumerator PerformActorMove(Actor actor, Map.Path path) {
        var soldiers = Map.instance.GetActors<Soldier>();
        for (int i = 1; i < path.nodes.Length; i++) {
            var tile = path.nodes[i].tile;

            // fire damage
            if (tile.onFire) {
                actor.MoveTo(tile);
                actor.TurnTo(tile.gridLocation - path.nodes[i - 1].tile.gridLocation);
                var damage = Random.Range(tile.fire.minDamage, tile.fire.maxDamage + 1);
                actor.Hurt(damage);
                yield return new WaitForSeconds(0.5f);
                if (actor.dead) break;
            }

            // trigger reactions
            foreach (var soldier in soldiers.Where(sol => sol.reaction != null)) {
                if (soldier.reaction.TriggersReaction(tile, actor)) {
                    actor.MoveTo(tile);
                    actor.TurnTo(tile.gridLocation - path.nodes[i - 1].tile.gridLocation);
                    CameraController.CentreCameraOn(tile);
                    yield return soldier.reaction.PerformReaction(tile);
                    if (actor.dead) break;
                }
            }
            if (actor.dead) break;
        }
        if (!actor.dead) {
            actor.MoveTo(path.last.tile);
            if (path.length >= 1) actor.TurnTo(path.last.tile.gridLocation - path.penultimate.tile.gridLocation);
        }
        
        // Alert aliens
        if (actor is Soldier) {
            MakeNoise(actor.gridLocation);
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

    public static IEnumerator PerformPickupChest(Soldier soldier, Tile tile) {
        tile.GetBackgroundActor<Chest>().Remove();
        // TODO add some UI to show what has been gained
        yield return new WaitForSeconds(1f);
        soldier.HighlightActions();
    }

    public static IEnumerator PerformExplosion(Soldier soldier, Tile tile) {
        var weapon = soldier.weapon;
        soldier.PlayAudio(weapon.audio.explosion);
        MakeNoise(tile.gridLocation);
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
                if (weapon.flames) {
                    if (randTile.onFire) {
                        if (weapon.minDamage + weapon.maxDamage > randTile.fire.minDamage + randTile.fire.maxDamage) {
                            randTile.fire.minDamage = weapon.minDamage;
                            randTile.fire.maxDamage = weapon.maxDamage;
                            randTile.fire.timer = weapon.flameDuration - iLayer;
                        } else {
                            randTile.fire.timer += 1;
                        }
                    } else {
                        var flameGO = Tile.Instantiate(Resources.Load<GameObject>("Prefabs/Flame")) as GameObject;
                        var flame = flameGO.GetComponent<Fire>();
                        flame.minDamage = weapon.minDamage;
                        flame.maxDamage = weapon.maxDamage;
                        flame.timer = weapon.flameDuration - iLayer;
                        randTile.SetFire(flame);
                    }
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