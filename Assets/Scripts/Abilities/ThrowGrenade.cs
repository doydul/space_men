using UnityEngine;
using System.Linq;
using System.Collections;

[CreateAssetMenu(fileName = "ThrowGrenade", menuName = "Abilities/Throw Grenade", order = 1)]
public class ThrowGrenade : LimitedUseAbility {
    
    public int scatterDistanceInterval = 5;
    
    public Weapon weapon;

    public override void Use() {
        AnimationManager.instance.StartAnimation(PerformUse());
    }

    private IEnumerator PerformUse() {
        AbilityInfoPanel.instance.ShowDescription("Throw Grenade\nChoose Target");
        SideModal.instance.ShowCollapsible(description);
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
        
        base.Use();
        var accuracy = weapon.accuracy;
        if (Random.value * 100 > accuracy) {
            // Miss
            var dist = Map.instance.ManhattanDistance(owner.gridLocation, MapInputController.instance.selectedTile.gridLocation);
            var maxScatterDist = (dist / scatterDistanceInterval) + 1;
            var scatterDist = Random.Range(1, maxScatterDist + 1);
            int currentLayer = 0;
            foreach (var layer in Map.instance.iterator.Exclude(new ExplosionImpassableTerrain()).EnumerateLayersFrom(hitTile.gridLocation)) {
                if (currentLayer == scatterDist) {
                    var randTile = layer.Where(tile => owner.CanSee(tile.gridLocation)).Sample();
                    if (randTile != null) hitTile = randTile;
                    break;
                }
                currentLayer++;
            }
        }
        
        var diff = hitTile.realLocation - owner.realLocation;
        var angle = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(diff.y, diff.x) - 90);
        yield return GameplayOperations.PerformTurnAnimation(owner, angle);
        
        owner.PlayAudio(weapon.audio.shoot);
        yield return SFXLayer.instance.PerformTracer(owner.centrePosition, hitTile.transform.position, weapon, true);
        yield return GameplayOperations.PerformExplosion(owner, hitTile, weapon);
        
        yield return GameplayOperations.PerformTurnAnimation(owner, Actor.FacingToDirection(hitTile.gridLocation - owner.gridLocation), true);
    }
}