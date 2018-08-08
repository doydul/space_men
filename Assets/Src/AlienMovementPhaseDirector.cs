using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AlienMovementPhaseDirector : MonoBehaviour { 
    
    private const float MOVEMENT_WAIT_TIME = 0.1f;
    private const float COMBAT_WAIT_TIME = 1f;
    
    public Map map;
    public Camera cam;
    
    public void MoveAliens(Action finished) {
        StartCoroutine(MoveAliensRoutine(finished));
    }
    
    private IEnumerator MoveAliensRoutine(Action callback) {
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
                        yield return StartCoroutine(MoveAlien(alien, location, direction));
                        yield return StartCoroutine(PerformAttack(alien));
                        break;
                    } else if (actor is Alien && unMovedAliens.Contains((Alien)actor)) {
                        if (alien != actor) {
                            remove = false;
                            break;
                        } else {
                            yield return StartCoroutine(PerformAttack(alien));
                        }
                    }
                }
                if (remove) unMovedAliens.Remove(alien);
            }
        }
        callback();
    }
    
    private void CentreCameraOn(Tile tile) {
        var temp = cam.transform.position;
        temp.x = tile.transform.position.x;
        temp.y = tile.transform.position.y;
        cam.transform.position = temp;
    }
    
    private IEnumerator MoveAlien(Alien alien, Vector2 destination, Vector2 direction) {
        CentreCameraOn(alien.tile);
        yield return new WaitForSeconds(MOVEMENT_WAIT_TIME);
        alien.MoveTo(map.GetTileAt(destination));
        alien.TurnTo(direction);
        CentreCameraOn(alien.tile);
        yield return new WaitForSeconds(MOVEMENT_WAIT_TIME);
    }
    
    private IEnumerator PerformAttack(Alien alien) {
        foreach (Tile tile in map.AdjacentTiles(alien.tile)) {
            var soldier = tile.GetActor<Soldier>();
            if (soldier != null) {
                CentreCameraOn(alien.tile);
                alien.Face(tile.gridLocation);
                alien.ShowAttackIndicator();
                AlienAttack.Execute(alien, soldier, map);
                yield return new WaitForSeconds(COMBAT_WAIT_TIME);
            }
        }
    }
}