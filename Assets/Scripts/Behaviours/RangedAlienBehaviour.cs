using UnityEngine;
using System.Linq;
using System.Collections;

[CreateAssetMenu(fileName = "RangedBehaviour", menuName = "Behaviours/Ranged", order = 1)]
public class RangedAlienBehaviour : AlienBehaviour {
    
    public int minDistance;
    public int maxDistance;
    public bool duckBack;
    public bool waitToSeeSoldier;
    public Weapon weaponProfile;
    
    bool hasSeenSoldier;
    bool notFirstTurn;
    
    public override IEnumerator PerformTurn() {
        if (!waitToSeeSoldier) hasSeenSoldier = true;
        
        var soldierPositions = Map.instance.GetActors<Soldier>().Select(soldier => soldier.gridLocation).ToArray();
        if (!hasSeenSoldier && soldierPositions.Where(pos => body.CanSee(pos)).Any()) hasSeenSoldier = true; // enable shooting if enemy was visible at start of turn

        Tile bestTile = null;
        Map.Path bestPath = null;
        bool bestTileJustRight = false;
        
        var currentDistance = Map.instance.ShortestPath(new AlienImpassableTerrain(), body.gridLocation, soldierPositions, true);
        if (!currentDistance.exists) yield break;
        bool tooClose = currentDistance.length < minDistance;
        
        foreach (var tile in Map.instance.iterator.Exclude(new AlienImpassableTerrain()).RadiallyFrom(body.gridLocation, body.remainingMovement)) {
            if (tile.occupied && tile.GetActor<Alien>() != body) continue;
            var path = Map.instance.ShortestPath(new AlienImpassableTerrain(), tile.gridLocation, soldierPositions, true);
            if (!path.exists) continue;
            
            bool canSeeEnemies = soldierPositions.Any(pos => body.CanSeeFrom(pos, tile.gridLocation));
            bool justRight = path.length >= minDistance && path.length <= maxDistance;
            
            if (justRight && canSeeEnemies) {
                bestTile = tile;
                break;
            }
            if (bestTileJustRight) continue;
            
            bool distanceCondition = bestPath == null || (tooClose ? path.length > bestPath.length : path.length < bestPath.length);
            
            if (bestPath == null || distanceCondition) {
                bestPath = path;
                bestTile = tile;
                bestTileJustRight = justRight;
            }
        }
        int tilesMoved = 0;        
        if (bestTile != null) {
            if (!body.tile.foggy) {
                CameraController.CentreCameraOn(body.tile);
                yield return new WaitForSeconds(0.5f);
            }
            var actualPath = Map.instance.ShortestPath(new AlienImpassableTerrain(), body.gridLocation, bestTile.gridLocation);
            tilesMoved = actualPath.length;
            yield return GameplayOperations.PerformActorMove(body, actualPath);
            if (!bestTile.foggy) {
                CameraController.CentreCameraOn(bestTile);
                yield return new WaitForSeconds(0.5f);
            }
        } else {
            yield break;
        }
        
        // attack
        if (!body.dead) {
            bool hasShot = false;
            var soldiersInLOS = soldierPositions.Where(pos => body.CanSee(pos));
            if (!soldiersInLOS.Any()) yield break;
            
            if (notFirstTurn && hasSeenSoldier) {
                var targetPos = soldiersInLOS.MinBy(pos => Map.instance.ManhattanDistance(pos, body.gridLocation));
                var target = Map.instance.GetTileAt(targetPos).GetActor<Soldier>();
                if (weaponProfile.InRange(body.gridLocation, targetPos)) {
                    yield return GameplayOperations.PerformAlienShoot(body, weaponProfile, target);
                    hasShot = true;
                }
            } else {
                hasSeenSoldier = true;
            }
            
            // duck back
            if (duckBack && hasShot && tilesMoved < body.remainingMovement) {
                bestTile = null;
                foreach (var tile in Map.instance.iterator.Exclude(new AlienImpassableTerrain()).RadiallyFrom(body.gridLocation, body.remainingMovement - tilesMoved)) {
                    if (tile.occupied) continue;
                    
                    bool canSeeEnemies = soldierPositions.Any(pos => body.CanSeeFrom(pos, tile.gridLocation));
                    if (!canSeeEnemies) {
                        bestTile = tile;
                        break;
                    }
                }
                if (bestTile != null) {
                    var actualPath = Map.instance.ShortestPath(new AlienImpassableTerrain(), body.gridLocation, bestTile.gridLocation);
                    yield return GameplayOperations.PerformActorMove(body, actualPath);
                }
            }
        }
        notFirstTurn = true;
    }
}