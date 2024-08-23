using UnityEngine;
using System.Linq;
using System.Collections;

[CreateAssetMenu(fileName = "ThrowGrenade", menuName = "Abilities/Throw Grenade", order = 1)]
public class ThrowGrenade : Ability {
    
    public int scatterDistanceInterval = 5;
    
    public int uses = 1;
    public Weapon weapon;

    public override bool CanUse() {
        return uses > 0;
    }

    public override void Use() {
        AnimationManager.instance.StartAnimation(PerformUse());
    }

    private IEnumerator PerformUse() {
        AbilityInfoPanel.instance.ShowDescription("Throw Grenade\nChoose Target");
        SideModal.instance.Show(description);
        yield return MapInputController.instance.SelectTileFrom(
            Color.red,
            Map.instance.iterator.Exclude(new SoldierLosMask())
                .RadiallyFrom(owner.gridLocation, 100)
                .Where(tile => owner.CanSee(tile.gridLocation) && weapon.InRange(owner.gridLocation, tile.gridLocation))
                .ToArray()
        );
        SideModal.instance.Hide();
        var hitTile = MapInputController.instance.selectedTile;
        if (hitTile == null) yield break;
        AbilityInfoPanel.instance.Hide();
        
        uses -= 1;
        var accuracy = weapon.accuracy;
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
        owner.PlayAudio(weapon.audio.shoot);
        yield return SFXLayer.instance.PerformTracer(owner.centrePosition, hitTile.transform.position, weapon, true);
        yield return GameplayOperations.PerformExplosion(owner, hitTile, weapon);
    }
}