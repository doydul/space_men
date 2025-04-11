using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Autoload", menuName = "Abilities/Autoload", order = 1)]
public class Autoload : LimitedUseAbility {
    
    public override IEnumerable<AbilityCondition> Conditions() {
        foreach (var con in base.Conditions()) yield return con;
        yield return new ClipNotFull();
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