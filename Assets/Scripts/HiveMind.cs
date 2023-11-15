using UnityEngine;
using System.Linq;
using System.Collections;

public class HiveMind : MonoBehaviour {

    Alien activeAlien;
    
    void Update() {
        if (UIState.instance.IsAlienTurn()) ContemplateMoves();
    }

    void ContemplateMoves() {
        if (activeAlien == null) {
            if (!ChooseActiveAlien()) {
                UIState.instance.StartPlayerTurn();
                GameEvents.Trigger("player_turn_start");
                return;
            }
            if (!AnimationManager.instance.animationInProgress) AnimationManager.instance.StartAnimation(PerformAlienMove());
        }
    }

    bool ChooseActiveAlien() {
        var list = Map.instance.GetActors<Alien>().Where(alien => alien.canAct).ToArray();
        if (list.Count() <= 0) return false;
        activeAlien = list[Random.Range(0, list.Count())];
        return true;
    }

    IEnumerator PerformAlienMove() {
        var soldierPositions = Map.instance.GetActors<Soldier>().Select(soldier => soldier.gridLocation).ToArray();
        Tile bestTile = null;
        Map.Path bestPath = null;
        foreach (var tile in Map.instance.iterator.Exclude(new AlienImpassableTerrain()).RadiallyFrom(activeAlien.gridLocation, activeAlien.movement)) {
            if (tile.occupied && tile.GetActor<Alien>() != activeAlien) continue;
            var path = Map.instance.ShortestPath(new AlienImpassableTerrain(), tile.gridLocation, soldierPositions, true);
            if (!path.exists) {
                break;
            }
            if (bestPath == null || path.length < bestPath.length ||
                    path.length == bestPath.length &&
                    Map.instance.ManhattanDistance(activeAlien.gridLocation, tile.gridLocation) < Map.instance.ManhattanDistance(activeAlien.gridLocation, bestTile.gridLocation)) {
                bestPath = path;
                bestTile = tile;
            }
        }
        if (bestTile != null) {
            if (!activeAlien.tile.foggy) {
                CameraController.CentreCameraOn(activeAlien.tile);
                yield return new WaitForSeconds(0.5f);
            }
            var actualPath = Map.instance.ShortestPath(new AlienImpassableTerrain(), activeAlien.gridLocation, bestTile.gridLocation);
            yield return GameplayOperations.PerformActorMove(activeAlien, actualPath);
            if (!bestTile.foggy) {
                CameraController.CentreCameraOn(bestTile);
                yield return new WaitForSeconds(0.5f);
            }

            foreach (var tile in Map.instance.AdjacentTiles(activeAlien.gridLocation)) {
                var soldier = tile.GetActor<Soldier>();
                if (soldier != null) {
                    CameraController.CentreCameraOn(tile);
                    yield return PerformAlienAttack(soldier);
                    break;
                }
            }
        } else {
            yield return null;
        }

        activeAlien.hasActed = true;
        activeAlien = null;
    }

    IEnumerator PerformAlienAttack(Soldier target) {
        activeAlien.ShowAttack();
        activeAlien.Face(target.gridLocation);
        yield return new WaitForSeconds(0.25f);
        BloodSplatController.instance.MakeSplat(target);
        yield return new WaitForSeconds(0.25f);
        target.Hurt(activeAlien.damage);
        activeAlien.HideAttack();
    }
}