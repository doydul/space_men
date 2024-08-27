using UnityEngine;
using System.Collections;
using System.Linq;

[CreateAssetMenu(fileName = "Sprint", menuName = "Abilities/Sprint", order = 1)]
public class Sprint : Ability {
    
    public override bool CanUse() {
        return owner.canAct;
    }

    public override void Use() {
        AnimationManager.instance.StartAnimation(PerformUse());
    }
    
    public IEnumerator PerformUse() {
        yield return ConfirmationPopup.instance.AskForConfirmation("sprint\n" + description);
        if (!ConfirmationPopup.instance.confirmed) yield break;
        
        owner.actionsSpent += 1;
        owner.tilesMoved -= owner.sprintMovement;
        owner.RefreshUI();
    }
}