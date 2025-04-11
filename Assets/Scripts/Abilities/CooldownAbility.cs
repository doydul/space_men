using UnityEngine;

public abstract class CooldownAbility : Ability {
    
    public int cooldown;
    
    protected int cooldownCounter;
    
    public override bool CanUse() {
        return cooldownCounter <= 0;
    }
    
    protected void SetCooldown() {
        cooldownCounter = cooldown;
    }
    
    public override void Setup() {
        GameEvents.On(this, "player_turn_start", () => {
            cooldownCounter--;
        });
    }
    
    public override void Teardown() {
        GameEvents.RemoveListener(this, "player_turn_start");
    }

    public override void Display(AbilityIcon icon) {
        if (cooldownCounter > 0) icon.centreText = cooldownCounter.ToString();
    }
}