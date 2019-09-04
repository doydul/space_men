using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AlienMovementPhaseDirector {
    
    public static AlienMovementPhaseDirector instance { get; private set; }

    private const float MOVEMENT_WAIT_TIME = 0.1f;
    private const float COMBAT_WAIT_TIME = 1f;

    public AlienMovementPhaseDirector(Map map, ICameraController cam, IAnimationReel animator) {
        instance = this;
        this.map = map;
        this.cam = cam;
        this.animator = animator;
    }

    Map map;
    ICameraController cam;
    IAnimationReel animator;

    public DelayedAction MoveAliens() {
        var result = new DelayedAction();
        Main.instance.StartCoroutine(MoveAliensRoutine(result));
        return result;
    }

    IEnumerator MoveAliensRoutine(DelayedAction delayedAction) {
        var aliensCopy = map.GetActors<Alien>();
        var wrapper = new AlienPathingMapWrapper(map, aliensCopy);
        while (aliensCopy.Count > 0) {
            int unmoved = aliensCopy.Count;
            foreach (var alien in new List<Alien>(aliensCopy)) {
                var output = new AlienPathFinder2(wrapper, new BasicAlienPathingWrapper(map)).BestMoveLocation(alien.gridLocation, alien.movement);
                var tile = map.GetTileAt(output.targetLocation);
                if (tile.actor == null || alien.tile == tile) {
                    aliensCopy.Remove(alien);
                    if (tile.actor == null) yield return Main.instance.StartCoroutine(MoveAlien(alien, output.targetLocation, output.facing));
                    yield return Main.instance.StartCoroutine(PerformAttack(alien));
                }
            }
            if (unmoved == aliensCopy.Count) break;
        }
        delayedAction.Finish();
    }

    IEnumerator MoveAlien(Alien alien, Vector2 destination, Vector2 direction) {
        if (!alien.tile.foggy) {
            cam.CentreCameraOn(alien.tile);
            yield return new WaitForSeconds(MOVEMENT_WAIT_TIME);
        }
        alien.MoveTo(map.GetTileAt(destination));
        alien.TurnTo(direction);
        if (!alien.tile.foggy) {
            cam.CentreCameraOn(alien.tile);
            yield return new WaitForSeconds(MOVEMENT_WAIT_TIME);
        }
    }

    IEnumerator PerformAttack(Alien alien) {
        foreach (Tile tile in map.AdjacentTiles(alien.tile)) {
            var soldier = tile.GetActor<Soldier>();
            if (soldier != null) {
                cam.CentreCameraOn(alien.tile);
                alien.Face(tile.gridLocation);
                yield return animator.PlayAlienAttackAnimation(alien);
                AlienAttack.Execute(alien, soldier, GameLogicComponent.world);
                yield return new WaitForSeconds(COMBAT_WAIT_TIME);
                yield break;
            }
        }
    }
}
