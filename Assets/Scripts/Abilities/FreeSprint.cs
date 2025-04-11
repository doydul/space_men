using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "FreeSprint", menuName = "Abilities/FreeSprint", order = 1)]
public class FreeSprint : CooldownAbility {
    
    public override IEnumerable<AbilityCondition> Conditions() {
        foreach (var con in base.Conditions()) yield return con;
        yield return new HasAction();
    }

    public override void Use() {
        AnimationManager.instance.StartAnimation(PerformUse());
    }
    
    public IEnumerator PerformUse() {
        if (Settings.confirmAbilities) {
            bool confirmed = false;
            yield return NotificationPopup.PerformShow(userFacingName, description, new BtnData("cancel", () => {}), new BtnData("ok", () => confirmed = true));
            if (!confirmed) yield break;
        }
        
        SetCooldown();
        owner.tilesMoved -= owner.sprintMovement;
    }
}