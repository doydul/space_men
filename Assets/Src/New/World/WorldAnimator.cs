using UnityEngine;
using System;
using System.Collections;

public class WorldAnimator : IAnimationReel {

    public WorldAnimator(WorldAnimation.IAnimationInteractor interactor) {
        this.interactor = interactor;
    }

    WorldAnimation.IAnimationInteractor interactor;

    public DelayedAction PlayStandardShootAnimation(
        Soldier shooter,
        Tile target,
        ShootingAnimationType animationType
    ) {
        var animation = new ShootingAnimation(
            shooter: shooter,
            target: target,
            type: animationType
        );
        return PlayAnimation(WorldState.instance, animation);
    }

    // public DelayedAction PlayOrdnanceShootAnimation(
    //     Soldier shooter,
    //     Tile target
    // ) {
    //     var animation = new ShootingOrdnanceAnimation(
    //         shooter: shooter,
    //         target: target
    //     );
    //     return PlayAnimation(WorldState.instance, animation);
    // }

    DelayedAction PlayAnimation(WorldState worldState, WorldAnimation animation) {
        var result = new DelayedAction();
        worldState.StartOfAnimation();
        interactor.StartCoroutine(animation.Animate(interactor, result));
        return result.Then(() => {
            worldState.EndOfAnimation();
        });
    }
}
