using UnityEngine;
using System.Linq;
using System.Collections;

[CreateAssetMenu(fileName = "StandardBehaviour", menuName = "Behaviours/Standard", order = 1)]
public class StandardAlienBehaviour : AlienBehaviour {
    
    [Range(0, 1f)]
    public float critChance = 1f/6f;
    
    public override IEnumerator PerformTurn() {
        var soldierPositions = Map.instance.GetActors<Soldier>().Select(soldier => soldier.gridLocation).ToArray();
        Tile bestTile = null;
        Map.Path bestPath = null;
        foreach (var tile in Map.instance.iterator.Exclude(new AlienImpassableTerrain()).RadiallyFrom(body.gridLocation, body.remainingMovement)) {
            if (tile.HasActor<Alien>() && tile.GetActor<Alien>() != body && tile.GetActor<Alien>().displacementPriority >= body.displacementPriority) continue;
            var path = Map.instance.ShortestPath(new AlienImpassableTerrain(), tile.gridLocation, soldierPositions, true);
            if (!path.exists) break;
            if (bestPath == null || path.length < bestPath.length ||
                    path.length == bestPath.length &&
                    Map.instance.ManhattanDistance(body.gridLocation, tile.gridLocation) < Map.instance.ManhattanDistance(body.gridLocation, bestTile.gridLocation)) {
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
            
            if (bestTile.HasActor<Alien>() && bestTile.GetActor<Alien>() != body) {
                // displace smaller alien
                var alien = bestTile.GetActor<Alien>();
                var closestTile = Map.instance.iterator.Exclude(new AlienImpassableTerrain()).RadiallyFrom(alien.gridLocation, 100).First(tile => !tile.occupied || tile == body.tile);
                yield return AnimationManager.instance.WaitForAll(
                    GameplayOperations.PerformActorMove(body, actualPath),
                    GameplayOperations.PerformActorMove(alien, Map.instance.ShortestPath(new AlienImpassableTerrain(), alien.gridLocation, closestTile.gridLocation), ignoreReactions: true)
                );
            } else {
                yield return GameplayOperations.PerformActorMove(body, actualPath);
            }
            
            if (!bestTile.foggy) {
                CameraController.CentreCameraOn(bestTile);
                yield return new WaitForSeconds(0.5f);
            }

            if (!body.dead) {
                foreach (var tile in Map.instance.AdjacentTiles(body.gridLocation)) {
                    var soldier = tile.GetActor<Soldier>();
                    if (soldier != null) {
                        CameraController.CentreCameraOn(tile);
                        yield return PerformAttack(soldier);
                        break;
                    }
                }
            }
        } else {
            yield return null;
        }
    }
    
    private IEnumerator PerformAttack(Soldier target) {
        SFXLayer.instance.SpawnBurst(target.realLocation, new Vector3(0, 1, 0), Resources.Load<ParticleBurst>("Prefabs/SFX/ParticleBursts/Slash"));
        body.Face(target.gridLocation);
        yield return new WaitForSeconds(0.25f);
        target.SpawnBlood();
        yield return new WaitForSeconds(0.25f);
        if (Random.value < critChance && !target.HasTrait(Trait.CritImmune)) {
            target.Hurt(body.damage * 2);
            Debug.Log("!!!CRITICAL HIT!!!");
        } else {
            target.Hurt(body.damage);
        }
    }
}