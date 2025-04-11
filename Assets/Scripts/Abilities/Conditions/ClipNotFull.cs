public class ClipNotFull : AbilityCondition {
    
    public override bool met => owner.shotsSpent > 0;
    public override string explanation => "ammo full";
}