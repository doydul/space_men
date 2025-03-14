using UnityEngine;

[CreateAssetMenu(fileName = "Shocked", menuName = "Status Effects/Shocked", order = 0)]
public class Shocked : StatusWithDuration {
    
    protected override void OnApply() {
        if (actor.HasTrait(Trait.ShockedImmune)) Remove();
    }
}