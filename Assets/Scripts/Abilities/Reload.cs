using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Reload", menuName = "Abilities/Reload", order = 1)]
public class Reload : Ability {
    
    public override bool CanUse() {
        return owner.shotsSpent > 0 && owner.canAct;
    }
    
    public override void Use() {
        AnimationManager.instance.StartAnimation(PerformUse());
    }
    
    public IEnumerator PerformUse() {
        yield return ConfirmationPopup.instance.AskForConfirmation("reload\n" + description);
        if (!ConfirmationPopup.instance.confirmed) yield break;
        
        owner.actionsSpent += 1;
        owner.shotsSpent = 0;
        owner.PlayAudio(owner.weapon.audio.reload);
        owner.RefreshUI();
    }
}