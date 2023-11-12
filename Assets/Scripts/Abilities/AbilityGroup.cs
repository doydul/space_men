using UnityEngine;

[CreateAssetMenu(fileName = "AbilityGroup", menuName = "Abilities/Ability Group", order = 0)]
public class AbilityGroup : Ability {

    public Ability[] abilities;

    public override void Attach(Soldier owner) {
        foreach (var ability in abilities) {
            ability.Attach(owner);
        }
    }
}