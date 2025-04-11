using UnityEngine;
using System.Collections;

public abstract class LimitedUseAbility : Ability {
    
    public int uses = 1;
    
    public override bool CanUse() {
        return uses > 0;
    }
    
    public override void Use() {
        uses--;
        if (uses <= 0) owner.abilities.Remove(this);
    }
    
    public override void Display(AbilityIcon icon) {
        icon.smallText = uses.ToString();
    }
}