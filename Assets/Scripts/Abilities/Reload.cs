using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Reload", menuName = "Abilities/Reload", order = 1)]
public class Reload : Ability {
    
    public override IEnumerable<AbilityCondition> Conditions() {
        yield return new ClipNotFull();
        yield return new HasAction();
    }
    
    public override void Use() {
        AnimationManager.instance.StartAnimation(PerformUse());
    }
    
    public IEnumerator PerformUse() {
        if (Settings.confirmAbilities) {
            bool confirmed = false;
            yield return NotificationPopup.PerformShow("reload", description, new BtnData("cancel", () => {}), new BtnData("ok", () => confirmed = true));
            if (!confirmed) yield break;
        }
        
        owner.actionsSpent += 1;
        owner.shotsSpent = 0;
        owner.PlayAudio(owner.weapon.audio.reload);
    }
}