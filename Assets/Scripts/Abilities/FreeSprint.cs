using UnityEngine;
using System.Collections;
using System.Linq;

[CreateAssetMenu(fileName = "FreeSprint", menuName = "Abilities/FreeSprint", order = 1)]
public class FreeSprint : CooldownAbility {
    
    public override bool CanUse() {
        return base.CanUse() && owner.canAct;
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