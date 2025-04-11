public class OffCooldown : AbilityCondition {
    
    public override bool met => (ability as CooldownAbility).cooldownCounter <= 0;
    public override string explanation => $"available in {(ability as CooldownAbility).cooldownCounter} turns";
}