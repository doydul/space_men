using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Autoload", menuName = "Abilities/Autoload", order = 1)]
public class Autoload : LimitedUseAbility {
    
    public override bool CanUse() {
        return base.CanUse() && owner.shotsSpent > 0;
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
        
        owner.shotsSpent = 0;
        owner.PlayAudio(owner.weapon.audio.reload);
        base.Use();
    }
}