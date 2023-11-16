using UnityEngine;
using System.Collections;
using System.Linq;

[CreateAssetMenu(fileName = "FullAuto", menuName = "Abilities/Full Auto", order = 1)]
public class FullAuto : Ability {

    public int ammoCost = 3;
    public int damage = 3;
    
    public override bool CanUse() {
        return owner.shotsRemaining >= ammoCost;
    }

    public override void Use() {
        AnimationManager.instance.StartAnimation(PerformUse());
    }

    private IEnumerator PerformUse() {
        AbilityInfoPanel.instance.ShowDescription("Full Auto\nChoose Target");
        yield return MapInputController.instance.SelectTileFrom(Color.red,
            Map.instance.GetActors<Alien>().Where(alien => !alien.tile.foggy && owner.CanSee(alien.gridLocation) && owner.InRange(alien.gridLocation)).Select(alien => alien.tile).ToArray()
        );
        if (MapInputController.instance.selectedTile == null) yield break;
        AbilityInfoPanel.instance.Hide();

        owner.shotsSpent += ammoCost;
        owner.Hurt(damage);
        yield return owner.PerformShoot(MapInputController.instance.selectedTile.GetActor<Alien>());
    }
}