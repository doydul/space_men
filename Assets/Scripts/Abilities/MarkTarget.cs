using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "MarkTarget", menuName = "Abilities/Mark Target", order = 1)]
public class MarkTarget : LimitedUseAbility {
    
    public StatusEffect status;
    
    private IEnumerable<Alien> possibleTargets => Map.instance.GetActors<Alien>().Where(alien => !alien.tile.foggy && owner.CanSee(alien.gridLocation));

    public override bool CanUse() {
        return base.CanUse() && possibleTargets.Any();
    }

    public override void Use() {
        AnimationManager.instance.StartAnimation(PerformUse());
    }

    private IEnumerator PerformUse() {
        AbilityInfoPanel.instance.ShowDescription("Mark Target\nChoose Target");
        SideModal.instance.ShowCollapsible(description);
        yield return MapInputController.instance.SelectTileFrom(
            Color.red,
            possibleTargets.Select(alien => alien.tile).ToArray()
        );
        SideModal.instance.Hide();
        var hitTile = MapInputController.instance.selectedTile;
        if (hitTile == null) yield break;
        AbilityInfoPanel.instance.Hide();
        
        status.Apply(hitTile.GetActor<Alien>());
        base.Use();
    }
}