using UnityEngine;
using System.Collections;

public class ShootingOrdnanceAnimation : WorldAnimation {

    Soldier shooter;
    Tile target;
    Explosion explosion;

    public ShootingOrdnanceAnimation(
        Soldier shooter,
        Tile target,
        Explosion explosion
    ) {
        this.shooter = shooter;
        this.target = target;
        this.explosion = explosion;
    }

    public override IEnumerator Animate(WorldAnimation.IAnimationInteractor interactor, DelayedAction delayedAction) {
        // var gunflareObject = interactor.MakeGunflare(shooter.muzzlePosition, shooter.rotation);
        // yield return new WaitForSeconds(0.1f);
        // var tracerObject = interactor.MakeTracer(shooter.muzzlePosition, target.realLocation);
        // yield return new WaitForSeconds(0.5f);
        // Object.Destroy(gunflareObject);
        yield return new WaitForSeconds(0.5f);
        delayedAction.Finish();
    }
}
