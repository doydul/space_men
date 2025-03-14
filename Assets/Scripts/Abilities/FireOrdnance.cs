using UnityEngine;
using System.Linq;
using System.Collections;

[CreateAssetMenu(fileName = "FireOrdnance", menuName = "Abilities/Fire Ordnance", order = 1)]
public class FireOrdnance : Ability {
    
    public int scatterDistanceInterval = 5;

    public override bool CanUse() {
        return owner.hasAmmo && owner.hasActions;
    }

    public override void Use() {
        AnimationManager.instance.StartAnimation(PerformUse());
    }

    private IEnumerator PerformUse() {
        AbilityInfoPanel.instance.ShowDescription("Fire Ordnance\nChoose Target");
        SideModal.instance.Show(description);
        yield return MapInputController.instance.SelectTileFrom(Color.red, Map.instance.iterator.Exclude(new SoldierLosMask()).RadiallyFrom(owner.gridLocation, 100)
          .Where(tile => owner.CanSee(tile.gridLocation) && owner.InRange(tile.gridLocation)).ToArray());
        var hitTile = MapInputController.instance.selectedTile;
        SideModal.instance.Hide();
        if (hitTile == null) yield break;
        AbilityInfoPanel.instance.Hide();
        
        owner.actionsSpent += 1;
        owner.shotsSpent += 1;
        if (owner.weapon.isHeavy) owner.tilesMoved += 100;
        
        yield return GameplayOperations.PerformSoldierFireOrdnance(owner, hitTile);
    }
}