using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "BlastBehaviour", menuName = "Behaviours/Blast", order = 1)]
public class BlastAlienBehaviour : AlienBehaviour {
    
    public int maxDistance;
    public int multitargetRadius;
    public Weapon weaponProfile;
    
    public override IEnumerator PerformTurn() {
        var soldierPositions = Map.instance.GetActors<Soldier>().Select(soldier => soldier.gridLocation).ToArray();
        Tile bestTile = null;
        Map.Path bestPath = null;
        
        var currentDistance = Map.instance.ShortestPath(new AlienImpassableTerrain(), body.gridLocation, soldierPositions, true);
        bool tooFar = currentDistance.length > maxDistance;
        
        foreach (var tile in Map.instance.iterator.Exclude(new AlienImpassableTerrain()).RadiallyFrom(body.gridLocation, body.remainingMovement)) {
            if (tile.occupied && tile.GetActor<Alien>() != body) continue;
            var path = Map.instance.ShortestPath(new AlienImpassableTerrain(), tile.gridLocation, soldierPositions, true);
            if (!path.exists) continue;
            
            bool canSeeEnemies = soldierPositions.Any(pos => body.CanSeeFrom(pos, tile.gridLocation));
            
            if (!tooFar && canSeeEnemies) {
                bestTile = tile;
                break;
            }
            
            if (bestPath == null || path.length < bestPath.length) {
                bestPath = path;
                bestTile = tile;
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
            
            // get all tiles within multitargetRadius from a soldier
            var targetablePositions = new HashSet<Vector2>();
            foreach (var soldierPos in soldiersInLOS) {
                foreach (var tile in Map.instance.iterator.RadiallyFrom(soldierPos, multitargetRadius)) {
                    if (body.CanSee(tile.gridLocation) && weaponProfile.InRange(body.gridLocation, tile.gridLocation)) targetablePositions.Add(tile.gridLocation);
                }
            }
            if (targetablePositions.Count() <= 0) yield break;
            
            // find the tile that will hit the most targets (or closest if there's a tie)
            Vector2 bestTargetPos = default(Vector2);
            int bestTargetDist = 0;
            int bestTargetHits = 0;
            foreach (var tilePos in targetablePositions) {
                int hits = 0;
                foreach (var tile in Map.instance.iterator.RadiallyFrom(tilePos, multitargetRadius)) {
                    if (tile.HasActor<Soldier>()) hits++;
                }
                int dist = Map.instance.ManhattanDistance(tilePos, body.gridLocation);
                if (bestTargetHits < hits || dist < bestTargetDist && hits == bestTargetHits) {
                    bestTargetPos = tilePos;
                    bestTargetDist = dist;
                    bestTargetHits = hits;
                }
            }
            
            var targetTile = Map.instance.GetTileAt(bestTargetPos);
            yield return GameplayOperations.PerformAlienOrdnance(body, weaponProfile, targetTile);
        }
    }
}