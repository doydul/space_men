using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "Overwatch", menuName = "Abilities/Overwatch", order = 1)]
public class Overwatch : ReactionAbility {
    
    public override bool CanUse() {
        return owner.hasAmmo && owner.canAct;
    }
    
    public override void Use() {
        AnimationManager.instance.StartAnimation(PerformUse());
    }
    
    public IEnumerator PerformUse() {
        if (Settings.confirmAbilities) {
            bool confirmed = false;
            yield return NotificationPopup.PerformShow("overwatch", description, new BtnData("cancel", () => {}), new BtnData("ok", () => confirmed = true));
            if (!confirmed) yield break;
        }
        
        owner.actionsSpent += 100;
        owner.tilesMoved += 100;
        owner.reaction = this;
        owner.ShowAbilityIcon(this);
        Tutorial.Show("overwatch");
    }

    public override bool TriggersReaction(Tile tile, Actor actor) {
        return !tile.foggy && owner.InRange(tile.gridLocation) && owner.CanSee(tile.gridLocation) && actor is Alien;
    }

    public override IEnumerator PerformReaction(Tile tile) {
        owner.HideAbilityIcon();
        owner.shotsSpent += 1;
        owner.reaction = null;
        yield return owner.PerformShoot(tile.GetActor<Alien>());
    }
}