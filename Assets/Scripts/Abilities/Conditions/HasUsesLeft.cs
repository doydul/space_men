public class HasUsesLeft : AbilityCondition {
    
    public override bool met => (ability as LimitedUseAbility).uses > 0;
    public override string explanation => "no uses left";
}