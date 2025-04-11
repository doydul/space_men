using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "Sprint", menuName = "Abilities/Sprint", order = 1)]
public class Sprint : Ability {
    
    public override IEnumerable<AbilityCondition> Conditions() {
        yield return new HasAction();
    }

    public override void Use() {
        AnimationManager.instance.StartAnimation(PerformUse());
    }
    
    public IEnumerator PerformUse() {
        if (Settings.confirmAbilities) {
            bool confirmed = false;
            yield return NotificationPopup.PerformShow("sprint", description, new BtnData("cancel", () => {}), new BtnData("ok", () => confirmed = true));
            if (!confirmed) yield break;
        }
        
        owner.actionsSpent += 1;
        owner.tilesMoved -= owner.sprintMovement;
    }
}