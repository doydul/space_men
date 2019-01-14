using UnityEngine;
using System.Collections;

public class AlienAttackAnimation : WorldAnimation {

    Alien attacker;

    public AlienAttackAnimation(Alien attacker) {
        this.attacker = attacker;
    }

    public override IEnumerator Animate(WorldAnimation.IAnimationInteractor interactor, DelayedAction delayedAction) {
        var attackMarkerObject = interactor.MakeAlienAttackMarker(attacker.realLocation + attacker.facing / 2);
        yield return new WaitForSeconds(0.5f);
        Object.Destroy(attackMarkerObject);
        delayedAction.Finish();
    }
}
