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
        var diff = target.realLocation - soldier.realLocation;
        var angle = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(diff.y, diff.x) - 90);
        yield return PerformTurnAnimation(soldier, angle);
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < soldier.shots; i++) {
            yield return PerformSoldierSingleShot(soldier, target);
        }
        yield return PerformTurnAnimation(soldier, Actor.FacingToDirection(target.gridLocation - soldier.gridLocation), true);
        soldier.RefreshUI();
    }

    public static IEnumerator PerformSoldierSingleShot(Soldier soldier, Alien target) {
        soldier.ShowMuzzleFlash();
        soldier.PlayAudio(soldier.weapon.audio.shoot);
        var accuracy = soldier.accuracy;
        if (!soldier.InHalfRange(target.gridLocation)) accuracy -= 15;
        if (!target.dead && Random.value * 100 <= accuracy + target.accModifier) {
            // HIT
            yield return SFXLayer.instance.PerformTracer(soldier.muzzlePosition, target.tile.transform.position, soldier.weapon, true);
            target.ShowHit();
            var damage = Random.Range(soldier.minDamage, soldier.maxDamage + 1);
            target.Hurt(damage, soldier.weapon.damageType);
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
                    yield return SFXLayer.instance.PerformTracer(soldier.muzzlePosition, actor.tile.transform.position, soldier.weapon, true);
                    var damage = Random.Range(soldier.minDamage, soldier.maxDamage + 1);
                    actor.Hurt(damage, soldier.weapon.damageType);
                    actor.ShowHit();
                    BloodSplatController.instance.MakeSplat(actor);
                    break;
                }
            }
            if (!secondaryHit) {
                yield return SFXLayer.instance.PerformTracer(soldier.muzzlePosition, target.tile.transform.position, soldier.weapon, false);
                soldier.PlayAudio(soldier.weapon.audio.impact.Sample());
            }
        }
        soldier.HideMuzzleFlash();
        yield return new WaitForSeconds(0.15f);
    }

    public static IEnumerator PerformActorMove(Actor actor, Map.Path path) {
        MapHighlighter.instance.ClearHighlights();
        var soldiers = Map.instance.GetActors<Soldier>();
        for (int i = 1; i < path.nodes.Length; i++) {
            var tile = path.nodes[i].tile;
            
            var facing = tile.gridLocation - path.nodes[i - 1].tile.gridLocation;
            yield return PerformTurnAnimation(actor, Actor.FacingToDirection(facing));
            yield return PerformMoveAnimation(actor, tile);
            
            
            if (!tile.occupied) {
                actor.MoveTo(tile);
                
                // fire damage
                if (tile.onFire) {
                    var damage = Random.Range(tile.fire.minDamage, tile.fire.maxDamage + 1);
                    actor.Hurt(damage);
                    yield return new WaitForSeconds(0.5f);
                    if (actor.dead) break;
                }

                // trigger reactions
                foreach (var soldier in soldiers.Where(sol => sol.reaction != null)) {
                    if (soldier.reaction.TriggersReaction(tile, actor)) {
                        CameraController.CentreCameraOn(tile);
                        yield return soldier.reaction.PerformReaction(tile);
                        if (actor.dead) break;
                    }
                }
                if (actor.dead) break;
            }
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
    
    public static IEnumerator PerformMoveAnimation(Actor actor, Tile destinationTile) {
        if (destinationTile.foggy) yield break;
        float duration = actor is Soldier ? 1f / ((Soldier)actor).baseMovement : 1f / ((Alien)actor).movement;
        var delta = destinationTile.foreground.position - actor.transform.position;
        var startTime = Time.time;
        var startPosition = actor.transform.localPosition;
        var targetPosition = startPosition + delta;
        while (Time.time - startTime < duration) {
            yield return null;
            float t = (Time.time - startTime) / duration;
            actor.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, t);
            // CameraController.CentreCameraOn(actor);
        }
    }
    
    public static IEnumerator PerformTurnAnimation(Actor actor, Quaternion desiredRotation) {
        if (actor.tile.foggy) yield break;
        float duration = actor is Soldier ? 1f / ((Soldier)actor).baseMovement : 1f / ((Alien)actor).movement;
        var startTime = Time.time;
        var startRotation = actor.image.transform.rotation;   
        while (Time.time - startTime < duration) {
            float t = (Time.time - startTime) / duration;
            actor.image.rotation = Quaternion.Slerp(startRotation, desiredRotation, t);
            yield return null;
        }
    }
    public static IEnumerator PerformTurnAnimation(Actor actor, Actor.Direction desiredFacing, bool force = false) {
        if (!force && actor.direction == desiredFacing) yield break;
        yield return PerformTurnAnimation(actor, Actor.DirectionToRotation(desiredFacing));
        actor.TurnTo(desiredFacing);
    }

    public static IEnumerator PerformOpenDoor(Soldier soldier, Tile tile) {
        tile.GetBackgroundActor<Door>().Remove();
        yield return new WaitForSeconds(1f);
        FogManager.instance.UpdateFog();
        soldier.HighlightActions();
    }

    public static IEnumerator PerformPickupChest(Soldier soldier, Tile tile) {
        var chest = tile.GetBackgroundActor<Chest>();
        ModalPopup.instance.ClearContent();
        if (chest.contents.hasItem) {
            PlayerSave.current.inventory.AddItem(chest.contents.item);
            string weaponOrArmour = chest.contents.item.isWeapon ? "a weapon" : "some armour";
            ModalPopup.instance.DisplayContent(Resources.Load<Transform>("Prefabs/UI/ItemReward")).GetComponent<ItemReward>().SetText($"you found {weaponOrArmour}\n{chest.contents.item.name}");
        }
        if (chest.contents.credits > 0) {
            PlayerSave.current.credits += chest.contents.credits;
            ModalPopup.instance.DisplayContent(Resources.Load<Transform>("Prefabs/UI/ItemReward")).GetComponent<ItemReward>().SetText($"you found credits\n{chest.contents.credits}");
        }
        chest.Remove();
        yield return new WaitForSeconds(1f);
        soldier.HighlightActions();
    }

    public static IEnumerator PerformExplosion(Soldier soldier, Tile tile, Weapon weapon = null) {
        weapon = weapon ?? soldier.weapon;
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