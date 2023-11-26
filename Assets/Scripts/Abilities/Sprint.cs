using UnityEngine;
using System.Collections;
using System.Linq;

[CreateAssetMenu(fileName = "Sprint", menuName = "Abilities/Sprint", order = 1)]
public class Sprint : Ability {
    
    public override bool CanUse() {
        return owner.canAct;
    }

    public override void Use() {
        owner.actionsSpent += 1;
        owner.tilesMoved -= owner.totalMovement;
        owner.RefreshUI();
    }
}