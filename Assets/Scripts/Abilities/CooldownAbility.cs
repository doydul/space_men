using UnityEngine;
using System.Collections.Generic;

public abstract class CooldownAbility : Ability {
    
    public int cooldown;
    
    public int cooldownCounter;
    
    public override IEnumerable<AbilityCondition> Conditions() {
        yield return new OffCooldown();
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