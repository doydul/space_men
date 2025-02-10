using UnityEngine;

[CreateAssetMenu(fileName = "MarkedTarget", menuName = "Status Effects/Marked Target", order = 0)]
public class MarkedTarget : StatusEffect {
    
    public float damageMultiplier = 0.5f;
    public float armourMultiplier = 0f;

    protected override void OnApply() {
        actor.damageMultiplier += damageMultiplier;
        actor.armourMultiplier = 0;
    }
}