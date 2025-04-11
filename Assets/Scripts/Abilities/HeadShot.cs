using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "HeadShot", menuName = "Abilities/Head Shot", order = 10)]
public class HeadShot : CooldownAbility {

    public Weapon weaponProfile;
    
    private IEnumerable<Tile> possibleTargets => Map.instance.GetActors<Alien>().Where(alien => !alien.tile.foggy && owner.CanSee(alien.gridLocation) && owner.InRange(alien.gridLocation)).Select(alien => alien.tile);
    
    public override IEnumerable<AbilityCondition> Conditions() {
        foreach (var con in base.Conditions()) yield return con;
        yield return new HasTarget(() => possibleTargets.Any());
        yield return new HasAction();
        yield return new HasAmmo();
    }

    public override void Use() {
        AnimationManager.instance.StartAnimation(PerformUse());
    }

    private IEnumerator PerformUse() {
        AbilityInfoPanel.instance.ShowDescription($"{userFacingName}\nChoose Target");
        SideModal.instance.ShowCollapsible(description);
        var tmp = owner.weapon;
        owner.weapon = weaponProfile;
        yield return MapInputController.instance.SelectTileFrom(Color.red,
            possibleTargets.ToArray()
        );
        SideModal.instance.Hide();
        if (MapInputController.instance.selectedTile == null) {
            owner.weapon = tmp;
            yield break;
        }
        AbilityInfoPanel.instance.Hide();
        
        SetCooldown();
        owner.shotsSpent += 1;
        owner.actionsSpent += 1;
        yield return owner.PerformShoot(MapInputController.instance.selectedTile.GetActor<Alien>());
        owner.weapon = tmp;
    }
}