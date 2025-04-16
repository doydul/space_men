using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class GameplayOperations {
    
    public const int SCATTER_DISTANCE_INTERVAL = 5;

    public static void MakeNoise(Vector2 gridLocation) {
        foreach (var alien in Map.instance.GetActors<Alien>()) {
            if (Map.instance.ManhattanDistance(gridLocation, alien.gridLocation) <= alien.sensoryRange) {
                alien.Awaken();
            }
        }
    }
    
    public static IEnumerator PerformSoldierShoot(Soldier soldier, Alien target) {
        var targetTile = target.tile;
        MakeNoise(target.gridLocation);
        var diff = target.realLocation - soldier.realLocation;
        var angle = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(diff.y, diff.x) - 90);
        soldier.AimAnimation();
        yield return PerformTurnAnimation(soldier, angle);
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < soldier.shots; i++) {
            yield return PerformSoldierSingleShot(soldier, target);
            if (target.dead) {
                // choose new target from adjacent enemies
                var possibleNewTargets = new List<Alien>();
                foreach (var tile in Map.instance.AdjacentTiles(targetTile, true)) {
                    if (tile.HasActor<Alien>()) {
                        possibleNewTargets.Add(tile.GetActor<Alien>());
                    }
                }
                if (possibleNewTargets.Count > 0) {
                    target = possibleNewTargets.Sample();
                    Debug.Log(target, target);
                }
            }
        }
        yield return PerformTurnAnimation(soldier, Actor.FacingToDirection(target.gridLocation - soldier.gridLocation), true);
        soldier.IdleAnimation();
        Tutorial.Show("actions");
    }

    public static IEnumerator PerformSoldierSingleShot(Soldier soldier, Alien target) {
        soldier.ShowMuzzleFlash();
        soldier.PlayAudio(soldier.weapon.audio.shoot);
        var accuracy = soldier.accuracy;
        if (!soldier.InHalfRange(target.gridLocation)) accuracy -= 15;
        if (!target.dead && Random.value * 100 <= accuracy + target.accModifier) {
            // HIT
            var damage = Random.Range(soldier.minDamage, soldier.maxDamage + 1);
            var effect = soldier.weapon.missEffect;
            if (target.DamageExceedsArmour(damage, soldier.weapon.damageType)) {
                effect = target.hitEffect;
            }
            yield return SFXLayer.instance.PerformTracer(soldier.muzzlePosition, target.tile.transform.position, soldier.weapon, true, new ParticleBurst[] { effect, soldier.weapon.impactEffect });
            target.Hurt(damage, soldier.weapon.damageType);
            if (!target.DamageExceedsArmour(damage, soldier.weapon.damageType)) soldier.PlayAudio(soldier.weapon.audio.impact.Sample());
            target.SpawnBlood();
        } else {
            // MISS
            var adjacentTiles = Map.instance.AdjacentTiles(target.tile, true).ToArray();
            bool secondaryHit = false;
            for (int j = 0; j < 2; j++) {
                var randTile = adjacentTiles[Random.Range(0, adjacentTiles.Length)];
                var actor = randTile.GetActor<Actor>();
                if (actor != null && actor != soldier) {
                    // SECONDARY HIT
                    secondaryHit = true;
                    var damage = Random.Range(soldier.minDamage, soldier.maxDamage + 1);
                    var effect = soldier.weapon.missEffect;
                    if (target.DamageExceedsArmour(damage, soldier.weapon.damageType)) {
                        effect = actor.hitEffect;
                    }
                    yield return SFXLayer.instance.PerformTracer(soldier.muzzlePosition, actor.tile.transform.position, soldier.weapon, true, new ParticleBurst[] { effect, soldier.weapon.impactEffect });
                    actor.Hurt(damage, soldier.weapon.damageType);
                    if (!actor.DamageExceedsArmour(damage, soldier.weapon.damageType)) soldier.PlayAudio(soldier.weapon.audio.impact.Sample());
                    actor.SpawnBlood();
                    break;
                }
            }
            if (!secondaryHit) {
                // TOTAL MISS
                yield return SFXLayer.instance.PerformTracer(soldier.muzzlePosition, target.tile.transform.position, soldier.weapon, false, new ParticleBurst[] { soldier.weapon.missEffect, soldier.weapon.impactEffect });
                soldier.PlayAudio(soldier.weapon.audio.impact.Sample());
            }
        }
        yield return new WaitForSeconds(0.15f);
    }
    
    public static IEnumerator PerformSoldierFireOrdnance(Soldier soldier, Tile targetTile) {
        var diff = targetTile.realLocation - soldier.realLocation;
        var angle = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(diff.y, diff.x) - 90);
        soldier.AimAnimation();
        yield return PerformTurnAnimation(soldier, angle);
        yield return new WaitForSeconds(0.2f);
        
        var hitTile = targetTile;
        var accuracy = soldier.accuracy;
        if (!soldier.InHalfRange(targetTile.gridLocation)) accuracy -= 15;
        if (Random.value * 100 > accuracy) {
            // Miss
            var dist = Map.instance.ManhattanDistance(soldier.gridLocation, MapInputController.instance.selectedTile.gridLocation);
            var maxScatterDist = (dist / SCATTER_DISTANCE_INTERVAL) + 1;
            var scatterDist = Random.Range(1, maxScatterDist + 1);
            int currentLayer = 0;
            foreach (var layer in Map.instance.iterator.Exclude(new ExplosionImpassableTerrain()).EnumerateLayersFrom(targetTile.gridLocation)) {
                if (currentLayer == scatterDist) {
                    var randTile = layer.Where(tile => soldier.CanSee(tile.gridLocation)).Sample();
                    if (randTile != null) hitTile = randTile;
                    break;
                }
                currentLayer++;
            }
        }
        soldier.PlayAudio(soldier.weapon.audio.shoot);
        soldier.ShowMuzzleFlash();
        yield return SFXLayer.instance.PerformTracer(soldier.muzzlePosition, hitTile.transform.position, soldier.weapon, true);
        yield return PerformExplosion(soldier, hitTile, soldier.weapon);
        
        yield return PerformTurnAnimation(soldier, Actor.FacingToDirection(targetTile.gridLocation - soldier.gridLocation), true);
        soldier.IdleAnimation();
    }
    
    public static IEnumerator PerformAlienShoot(Alien alien, Weapon weapon, Soldier target) {
        var targetTile = target.tile;
        var diff = target.realLocation - alien.realLocation;
        var angle = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(diff.y, diff.x) - 90);
        CameraController.CentreCameraOn(false, alien.transform, target.transform);
        yield return PerformTurnAnimation(alien, angle);
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < weapon.shots; i++) {
            yield return PerformAlienSingleShot(alien, weapon, target);
            if (target.dead) break;
        }
        yield return PerformTurnAnimation(alien, Actor.FacingToDirection(target.gridLocation - alien.gridLocation), true);
    }
    
    public static IEnumerator PerformAlienSingleShot(Alien alien, Weapon weapon, Soldier target) {
        alien.PlayAudio(weapon.audio.shoot);
        yield return SFXLayer.instance.PerformTracer(alien.muzzlePosition, target.tile.transform.position, weapon, true, new ParticleBurst[] { target.hitEffect, weapon.impactEffect });
        var damage = Random.Range(weapon.minDamage, weapon.maxDamage + 1);
        target.Hurt(damage, weapon.damageType);
        target.SpawnBlood();
        yield return new WaitForSeconds(0.15f);
    }
    
    public static IEnumerator PerformAlienOrdnance(Alien alien, Weapon weapon, Tile targetTile) {
        var diff = targetTile.realLocation - alien.realLocation;
        var angle = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(diff.y, diff.x) - 90);
        CameraController.CentreCameraOn(targetTile);
        yield return PerformTurnAnimation(alien, angle);
        alien.PlayAudio(weapon.audio.shoot);
        yield return SFXLayer.instance.PerformTracer(alien.muzzlePosition, targetTile.transform.position, weapon, true);
        yield return PerformExplosion(alien, targetTile, weapon);
        yield return PerformTurnAnimation(alien, Actor.FacingToDirection(targetTile.gridLocation - alien.gridLocation), true);
    }

    public static IEnumerator PerformActorMove(Actor actor, Map.Path path, bool ignoreReactions = false) {
        MapHighlighter.instance.ClearHighlights();
        var soldiers = Map.instance.GetActors<Soldier>();
        bool stoppedEarly = false;
        for (int i = 1; i < path.nodes.Length; i++) {
            actor.actualTilesMoved++;
            var tile = path.nodes[i].tile;
            
            var facing = tile.gridLocation - path.nodes[i - 1].tile.gridLocation;
            yield return PerformTurnAnimation(actor, Actor.FacingToDirection(facing));
            yield return PerformMoveAnimation(actor, tile);
            
            
            if (!tile.occupied) {
                actor.MoveTo(tile);
                if (actor is Soldier) FogManager.instance.UpdateFog();
                
                // fire damage
                if (tile.onFire) {
                    var damage = Random.Range(tile.fire.minDamage, tile.fire.maxDamage + 1);
                    actor.Hurt(damage);
                    yield return new WaitForSeconds(0.5f);
                    if (actor.dead) break;
                }

                // trigger reactions
                if (!ignoreReactions) {
                    foreach (var reaction in soldiers.Where(sol => sol.reaction != null).Select(sol => sol.reaction).OrderBy(reac => reac.reactionPriority)) {
                        if (reaction.TriggersReaction(tile, actor)) {
                            CameraController.CentreCameraOn(tile);
                            yield return reaction.PerformReaction(tile);
                            // experimental; this way, at most one reaction triggered per tile moved
                            break;
                            // if (actor.dead) break; 
                        }
                    }
                }
                if (actor.dead) break;
                if (actor.HasStatus<Shocked>() && actor.remainingMovement <= 0) {
                    stoppedEarly = true;
                    break;
                }
            }
        }
        if (!actor.dead && !stoppedEarly) {
            actor.MoveTo(path.last.tile);
            if (path.length >= 1) actor.TurnTo(path.last.tile.gridLocation - path.penultimate.tile.gridLocation);
        }
        
        if (actor is Soldier) {
            MakeNoise(actor.gridLocation);
            Tutorial.Show("movement");
            Tutorial.Show(Map.instance.GetTileAt(((GetToTarget)Objectives.current.objectives.Where(objective => objective is GetToTarget).First()).room.centre).transform, "objectives1");
            if (GameObject.Find("ShowObjectivesButton") != null) {
                Tutorial.Show(GameObject.Find("ShowObjectivesButton").transform, "objectives2", true, true);
            } else {
                Tutorial.Record("objectives2");
            }
            var adjacentDoorTile = Map.instance.AdjacentTiles(actor.tile).FirstOrDefault(tile => tile.HasActor<Door>());
            if (adjacentDoorTile != null) Tutorial.Show(adjacentDoorTile.transform, "doors");
        }
    }
    
    public static IEnumerator PerformMoveAnimation(Actor actor, Tile destinationTile) {
        if (destinationTile.foggy) yield break;
        actor.PlayAudio(actor.walkSounds.Sample());
        actor.MoveAnimation();
        float duration = actor is Soldier ? 1f / ((Soldier)actor).baseMovement : 1f / ((Alien)actor).movement;
        var startTime = Time.time;
        var startLocalPosition = actor.transform.localPosition;
        while (Time.time - startTime < duration) {
            yield return null;
            float t = (Time.time - startTime) / duration;
            actor.transform.position = Vector3.Lerp(actor.transform.parent.TransformPoint(startLocalPosition), destinationTile.foreground.position, MathUtil.EaseCubic(t));
            // CameraController.CentreCameraOn(actor);
        }
        actor.StationaryAnimation();
    }
    
    public static IEnumerator PerformTurnAnimation(Actor actor, Quaternion desiredRotation) {
        if (actor.tile.foggy) yield break;
        actor.MoveAnimation();
        float duration = actor is Soldier ? 1f / ((Soldier)actor).baseMovement : 1f / ((Alien)actor).movement;
        var startTime = Time.time;
        var startRotation = actor.image.transform.rotation;   
        while (Time.time - startTime < duration) {
            float t = (Time.time - startTime) / duration;
            actor.image.rotation = Quaternion.Slerp(startRotation, desiredRotation, MathUtil.EaseCubic(t));
            yield return null;
        }
        actor.StationaryAnimation();
    }
    public static IEnumerator PerformTurnAnimation(Actor actor, Actor.Direction desiredFacing, bool force = false) {
        if (actor == null || !force && actor.direction == desiredFacing) yield break;
        yield return PerformTurnAnimation(actor, Actor.DirectionToRotation(desiredFacing));
        actor.TurnTo(desiredFacing);
    }

    public static IEnumerator PerformOpenDoor(Soldier soldier, Tile tile) {
        yield return PerformTurnAnimation(soldier, Actor.FacingToDirection(tile.gridLocation - soldier.gridLocation));
        tile.GetBackgroundActor<Door>().Remove();
        yield return new WaitForSeconds(0.5f);
        FogManager.instance.UpdateFog();
    }

    public static IEnumerator PerformPickupChest(Soldier soldier, Tile tile) {
        yield return PerformTurnAnimation(soldier, Actor.FacingToDirection(tile.gridLocation - soldier.gridLocation));
        yield return new WaitForSeconds(0.2f);
        var chest = tile.GetBackgroundActor<Chest>();
        ModalPopup.instance.ClearContent();
        if (chest.contents.hasItem) {
            var item = chest.contents.item;
            PlayerSave.current.inventory.AddItem(chest.contents.item);
            string weaponOrArmour = item.isWeapon ? "a weapon" : "some armour";
            // ModalPopup.instance.DisplayContent(Resources.Load<Transform>("Prefabs/UI/ItemReward")).GetComponent<ItemReward>().SetText($"you found {weaponOrArmour}\n{chest.contents.item.name}");
            var itemInfoBox = ModalPopup.instance.DisplayContentInContainer(Resources.Load<GameObject>("Prefabs/UI/ItemInfoBox")).GetComponent<ItemInfoBox>();
            if (item.isWeapon) itemInfoBox.SetItem(Weapon.Get(item.name));
            else itemInfoBox.SetItem(Armour.Get(item.name));
        }
        if (chest.contents.credits > 0) {
            PlayerSave.current.credits += chest.contents.credits;
            ModalPopup.instance.DisplayContent(Resources.Load<Transform>("Prefabs/UI/ItemReward")).GetComponent<ItemReward>().SetText($"you found credits\n{chest.contents.credits}");
        }
        chest.Open();
    }

    public static IEnumerator PerformExplosion(Actor player, Tile tile, Weapon weapon) {
        player.PlayAudio(weapon.audio.explosion);
        MakeNoise(tile.gridLocation);
        float remainingBlast = weapon.blast;
        int iLayer = 0;
        foreach (var layer in Map.instance.iterator.Exclude(new ExplosionImpassableTerrain()).EnumerateLayersFrom(tile.gridLocation)) {
            remainingBlast -= iLayer * 2;
            iLayer++;
            while (layer.Count() > 0 && remainingBlast > 0) {
                remainingBlast -= 1;
                var randex = Random.Range(0, layer.Count());
                var randTile = layer[randex];
                layer.RemoveAt(randex);

                SFXLayer.instance.SpawnBurst(randTile.realLocation, Vector3.up, weapon.explosion.explosionVFX);
                var actor = randTile.GetActor<Actor>();
                if (actor != null) {
                    var damage = Random.Range(weapon.minDamage, weapon.maxDamage + 1);
                    actor.Hurt(damage);
                    actor.SpawnBlood();
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
                        var fire = Tile.Instantiate(weapon.explosion.fireVFX);
                        fire.minDamage = weapon.minDamage;
                        fire.maxDamage = weapon.maxDamage;
                        fire.timer = weapon.flameDuration - iLayer;
                        randTile.SetFire(fire);
                    }
                }
                if (weapon.explosion.hasDecals) DecalController.instance.SpawnDecal(randTile, weapon.explosion.GetDecal());
            }
        }
        yield return new WaitForSeconds(0.5f);
    }
}

public class ExplosionImpassableTerrain : IMask {
    public bool Contains(Tile tile) {
        return !tile.open || tile.GetBackgroundActor<Door>() != null;
    }
}