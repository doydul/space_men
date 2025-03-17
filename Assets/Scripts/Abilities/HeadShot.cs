using UnityEngine;
using System.Collections;
using System.Linq;

[CreateAssetMenu(fileName = "HeadShot", menuName = "Abilities/Head Shot", order = 10)]
public class HeadShot : CooldownAbility {

    public Weapon weaponProfile;
    
    public override bool CanUse() {
        return base.CanUse() && owner.hasActions && owner.shotsRemaining >= 1;
    }

    public override void Use() {
        AnimationManager.instance.StartAnimation(PerformUse());
    }

    private IEnumerator PerformUse() {
        AbilityInfoPanel.instance.ShowDescription($"{userFacingName}\nChoose Target");
        SideModal.instance.Show(description);
        var tmp = owner.weapon;
        owner.weapon = weaponProfile;
        yield return MapInputController.instance.SelectTileFrom(Color.red,
            Map.instance.GetActors<Alien>().Where(alien => !alien.tile.foggy && owner.CanSee(alien.gridLocation) && owner.InRange(alien.gridLocation)).Select(alien => alien.tile).ToArray()
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