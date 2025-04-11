public class HasAmmo : AbilityCondition {
    
    public override bool met => owner.shotsRemaining >= 1;
    public override string explanation => "not enough ammo";
}