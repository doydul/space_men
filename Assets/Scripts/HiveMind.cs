using UnityEngine;
using System.Linq;
using System.Collections;

public class HiveMind : MonoBehaviour {

    int threat;
    Alien activeAlien;

    void Update() {
        if (UIState.instance.IsAlienTurn()) ContemplateMoves();
    }

    private void ContemplateMoves() {
        if (activeAlien == null) {
            if (!ChooseActiveAlien()) {
                UIState.instance.StartPlayerTurn();
                GameEvents.Trigger("player_turn_start");
                Spawn();
                return;
            }
            if (!AnimationManager.instance.animationInProgress) AnimationManager.instance.StartAnimation(PerformAlienMove());
        }
    }

    private bool ChooseActiveAlien() {
        var list = Map.instance.GetActors<Alien>().Where(alien => alien.canAct).ToArray();
        if (list.Count() <= 0) return false;
        activeAlien = list[Random.Range(0, list.Count())];
        return true;
    }

    private IEnumerator PerformAlienMove() {
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

            if (!activeAlien.dead) {
                foreach (var tile in Map.instance.AdjacentTiles(activeAlien.gridLocation)) {
                    var soldier = tile.GetActor<Soldier>();
                    if (soldier != null) {
                        CameraController.CentreCameraOn(tile);
                        yield return PerformAlienAttack(soldier);
                        break;
                    }
                }
            }
        } else {
            yield return null;
        }

        activeAlien.hasActed = true;
        activeAlien = null;
    }

    private IEnumerator PerformAlienAttack(Soldier target) {
        activeAlien.ShowAttack();
        activeAlien.Face(target.gridLocation);
        yield return new WaitForSeconds(0.25f);
        BloodSplatController.instance.MakeSplat(target);
        yield return new WaitForSeconds(0.25f);
        target.Hurt(activeAlien.damage);
        activeAlien.HideAttack();
    }

    private void Spawn() {
        threat += 20;
        var spawners = Map.instance.spawners;
        while (threat > 0) {
            var profile = EnemyProfile.GetAll().Where(prof => prof.difficultyLevel == 1 && prof.spawnable && prof.threat <= threat).WeightedSelect();
            if (profile.threat <= 0) break;
            var randex = Random.Range(0, spawners.Length);
            InstantiatePod(profile.typeName, profile.count, spawners[randex].gridLocation, true);
            threat -= profile.threat;
        }
    }

    private void InstantiatePod(string type, int number, Vector2 gridLocation, bool awaken) {
        int counter = 0;
        var pod = new Alien.Pod();
        foreach (var node in Map.instance.iterator.Exclude(new AlienImpassableTerrain()).EnumerateFrom(gridLocation)) {
            if (node.tile.occupied) continue;
            var alien = InstantiateAlien(node.tile.gridLocation, type);
            pod.members.Add(alien);
            alien.pod = pod;
            if (awaken) alien.Awaken();
            counter++;
            if (counter >= number) break;
        }
    }

    private Alien InstantiateAlien(Vector2 gridLocation, string alienType) {
        var trans = MonoBehaviour.Instantiate(Resources.Load<Transform>("Prefabs/Alien")) as Transform;

        var alienData = Resources.Load<AlienData>($"Aliens/{alienType}");
        var alien = trans.GetComponent<Alien>() as Alien;
        alienData.Dump(alien);
        alien.id = System.Guid.NewGuid().ToString();

        var spriteTransform = Instantiate(Resources.Load<Transform>("Prefabs/AlienSprites/" + alienType + "AlienSprite")) as Transform;
        spriteTransform.parent = alien.image;
        spriteTransform.localPosition = Vector3.zero;

        Map.instance.GetTileAt(gridLocation).SetActor(trans);
        return alien;
    }
}