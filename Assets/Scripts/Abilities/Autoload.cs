using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Autoload", menuName = "Abilities/Autoload", order = 1)]
public class Autoload : Ability {
    
    public int uses = 1;
    
    public override bool CanUse() {
        return owner.shotsSpent > 0 && uses > 0;
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
        uses -= 1;
        if (uses <= 0) owner.abilities.Remove(this);
        owner.PlayAudio(owner.weapon.audio.reload);
    }
}