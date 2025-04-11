using System;

public class HasTarget : AbilityCondition {
    
    Func<bool> condition;
    
    public HasTarget(Func<bool> condition) {
        this.condition = condition;
    }
    
    public override bool met => condition();
    public override string explanation => $"no targets";
}