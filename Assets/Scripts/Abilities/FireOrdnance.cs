using UnityEngine;
using System.Linq;
using System.Collections;

[CreateAssetMenu(fileName = "FireOrdnance", menuName = "Abilities/Fire Ordnance", order = 1)]
public class FireOrdnance : Ability {
    
    public int scatterDistanceInterval = 5;

    public override bool CanUse() {
        return owner.hasAmmo && owner.canAct;
    }

    public override void Use() {
        AnimationManager.instance.StartAnimation(PerformUse());
    }

    private IEnumerator PerformUse() {
        owner.actionsSpent += 1;
        owner.shotsSpent += 1;
        yield return MapInputController.instance.SelectTileFrom(Color.red, Map.instance.iterator.Exclude(new SoldierLosMask()).RadiallyFrom(owner.gridLocation, 100)
          .Where(tile => owner.CanSee(tile.gridLocation) && owner.InRange(tile.gridLocation)).ToArray());
        var hitTile = MapInputController.instance.selectedTile;
        var accuracy = owner.accuracy;
        if (!owner.InHalfRange(hitTile.gridLocation)) accuracy -= 15;
        if (Random.value * 100 > accuracy) {
            // Miss
            var dist = Map.instance.ManhattanDistance(owner.gridLocation, MapInputController.instance.selectedTile.gridLocation);
            var maxScatterDist = (dist / scatterDistanceInterval) + 1;
            var scatterDist = Random.Range(1, maxScatterDist + 1);
            int currentLayer = 0;
            foreach (var layer in Map.instance.iterator.Exclude(new ExplosionImpassableTerrain()).EnumerateLayersFrom(hitTile.gridLocation)) {
                if (currentLayer == scatterDist) {
                    hitTile = layer[Random.Range(0, layer.Count())];
                    break;
                }
                currentLayer++;
            }
        }
        owner.Face(MapInputController.instance.selectedTile.gridLocation);
        owner.ShowMuzzleFlash();
        yield return new WaitForSeconds(0.2f);
        owner.HideMuzzleFlash();
        yield return GameplayOperations.PerformExplosion(owner.weapon, hitTile);
    }
}