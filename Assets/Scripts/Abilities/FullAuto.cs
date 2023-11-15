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
        owner.shotsSpent += ammoCost;
        foreach (var alien in Map.instance.GetActors<Alien>()) {
            if (owner.CanSee(alien.gridLocation)) {
                MapHighlighter.instance.HighlightTile(alien.tile, Color.red);
            }
        }
        yield return MapInputController.instance.SelectTileFrom(Color.red,
            Map.instance.GetActors<Alien>().Where(alien => owner.CanSee(alien.gridLocation) && owner.InRange(alien.gridLocation)).Select(alien => alien.tile).ToArray()
        );
        owner.Hurt(damage);
        yield return owner.PerformShoot(MapInputController.instance.selectedTile.GetActor<Alien>());
    }
}