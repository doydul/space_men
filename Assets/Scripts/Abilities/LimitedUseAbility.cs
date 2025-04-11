using System.Collections.Generic;
using System.Linq;

public abstract class LimitedUseAbility : Ability {
    
    public int uses = 1;

    public override IEnumerable<AbilityCondition> Conditions() {
        yield return new HasUsesLeft();
    }
    
    public override void Use() {
        uses--;
        if (uses <= 0) owner.abilities.Remove(this);
    }
    
    public override void Display(AbilityIcon icon) {
        icon.smallText = uses.ToString();
    }
}