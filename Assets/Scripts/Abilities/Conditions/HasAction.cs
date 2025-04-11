public class HasAction : AbilityCondition {
    
    public override bool met => owner.hasActions;
    public override string explanation => "not enough actions points";
}