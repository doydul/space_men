using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AlienMovementPhaseDirector {

    private const float MOVEMENT_WAIT_TIME = 0.1f;
    private const float COMBAT_WAIT_TIME = 1f;

    public AlienMovementPhaseDirector(Map map, ICameraController cam) {
        this.map = map;
        this.cam = cam;
    }

    Map map;
    ICameraController cam;

    public void MoveAliens(Action finished) {
        Main.instance.StartCoroutine(MoveAliensRoutine(finished));
    }

    private IEnumerator MoveAliensRoutine(Action finished) {
        var targets = map.GetActors<Soldier>().Select(soldier => soldier.gridLocation).ToList();
        var wrapper = new AlienPathingWrapper(map);
        var unMovedAliens = map.GetActors<Alien>();
        while (unMovedAliens.Any()) {
            var aliensCopy = new List<Alien>(unMovedAliens);
            foreach (var alien in aliensCopy) {
                var path = new Path(new PathFinder(wrapper, alien.gridLocation, targets).FindPath());

                bool remove = true;
                var pathSegment = path.FirstReverse(alien.movement + 1);
                for (int i = 0; i < pathSegment.Count; i++) {
                    Vector2 location = pathSegment[i];
                    var actor = map.GetActorAt<Actor>(location);
                    if (actor == null) {
                        var direction = location - pathSegment[i + 1];
                        yield return Main.instance.StartCoroutine(MoveAlien(alien, location, direction));
                        yield return Main.instance.StartCoroutine(PerformAttack(alien));
                        break;
                    } else if (actor is Alien && unMovedAliens.Contains((Alien)actor)) {
                        if (alien != actor) {
                            remove = false;
                            break;
                        } else {
                            yield return Main.instance.StartCoroutine(PerformAttack(alien));
                        }
                    }
                }
                if (remove) unMovedAliens.Remove(alien);
            }
        }
        finished();
    }

    private IEnumerator MoveAlien(Alien alien, Vector2 destination, Vector2 direction) {
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

    private IEnumerator PerformAttack(Alien alien) {
        foreach (Tile tile in map.AdjacentTiles(alien.tile)) {
            var soldier = tile.GetActor<Soldier>();
            if (soldier != null) {
                cam.CentreCameraOn(alien.tile);
                alien.Face(tile.gridLocation);
                alien.ShowAttackIndicator();
                AlienAttack.Execute(alien, soldier, GameLogicComponent.world);
                yield return new WaitForSeconds(COMBAT_WAIT_TIME);
            }
        }
    }
}
