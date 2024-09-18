using UnityEngine;
using System.Linq;
using System.Collections;

[CreateAssetMenu(fileName = "RangedBehaviour", menuName = "Behaviours/Ranged", order = 1)]
public class RangedAlienBehaviour : AlienBehaviour {
    
    [Range(0, 1f)]
    public float critChance = 1f/6f;
    public int minDistance;
    public int maxDistance;
    public Weapon weaponProfile;
    
    public override IEnumerator PerformTurn() {
        var soldierPositions = Map.instance.GetActors<Soldier>().Select(soldier => soldier.gridLocation).ToArray();
        Tile bestTile = null;
        Map.Path bestPath = null;
        bool bestTileJustRight = false;
        
        var currentDistance = Map.instance.ShortestPath(new AlienImpassableTerrain(), body.gridLocation, soldierPositions, true);
        bool tooFar = currentDistance.length > maxDistance;
        
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
            
            bool distanceCondition = bestPath == null || (tooFar ? path.length < bestPath.length : path.length > bestPath.length);
            
            if (bestPath == null || distanceCondition) {
                bestPath = path;
                bestTile = tile;
                bestTileJustRight = justRight;
            }
        }
        if (bestTile != null) {
            if (!body.tile.foggy) {
                CameraController.CentreCameraOn(body.tile);
                yield return new WaitForSeconds(0.5f);
            }
            var actualPath = Map.instance.ShortestPath(new AlienImpassableTerrain(), body.gridLocation, bestTile.gridLocation);
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
            var soldiersInLOS = soldierPositions.Where(pos => body.CanSee(pos));
            if (soldiersInLOS.Count() <= 0) yield break;
            var targetPos = soldiersInLOS.MinBy(pos => Map.instance.ManhattanDistance(pos, body.gridLocation));
            var target = Map.instance.GetTileAt(targetPos).GetActor<Soldier>();
            yield return GameplayOperations.PerformAlienShoot(body, weaponProfile, target);
        }
    }
}