using UnityEngine;

[CreateAssetMenu(fileName = "Reload", menuName = "Abilities/Reload", order = 1)]
public class Reload : Ability {
    
    public override bool CanUse() {
        return owner.shotsSpent > 0 && owner.canAct;
    }

    public override void Use() {
        owner.actionsSpent += 1;
        owner.shotsSpent = 0;
    }
}