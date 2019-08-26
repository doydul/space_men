using UnityEngine;
using System.Collections;

public class ShootingAnimation : WorldAnimation {

    const float projectileRandomization = 1;
    const float missDistance = 5;

    Soldier shooter;
    Tile target;
    ShootingAnimationType type;

    public ShootingAnimation(
        Soldier shooter,
        Tile target,
        ShootingAnimationType type
    ) {
        this.shooter = shooter;
        this.target = target;
        this.type = type;
    }

    public override IEnumerator Animate(WorldAnimation.IAnimationInteractor interactor, DelayedAction delayedAction) {
        var gunflareObject = interactor.MakeGunflare(shooter.muzzlePosition, shooter.rotation);
        // yield return new WaitForSeconds(0.1f);
        // var tracer = interactor.MakeTracer(shooter.muzzlePosition, shooter.weaponName);
        // tracer.StartAnimating(shooter.muzzlePosition, EndPosition()).Then(() => {
        //     if (type == ShootingAnimationType.Hit) {
        //         var impact = interactor.MakeTracerImpact(tracer.realLocation);
        //         Object.Destroy(impact, 0.5f);
        //     }
        //     tracer.Destroy();
        // });
        yield return new WaitForSeconds(0.1f);
        var impact = interactor.MakeTracerImpact(EndPosition());
        Object.Destroy(impact, 0.5f);
        yield return new WaitForSeconds(0.5f);
        Object.Destroy(gunflareObject);
        delayedAction.Finish();
    }

    Vector2 EndPosition() {
        if (type != ShootingAnimationType.Missed) {
            return MungedPosition(target.realLocation);
        } else {
            Vector2 projectileDirection = (target.realLocation - shooter.muzzlePosition).normalized;
            Vector2 extrapolatedPosition = target.realLocation + projectileDirection * missDistance;
            return MungedPosition(extrapolatedPosition);
        }
    }

    Vector2 MungedPosition(Vector2 preMungedValue) {
        return new Vector2(
            preMungedValue.x + Random.value * projectileRandomization - projectileRandomization / 2,
            preMungedValue.y + Random.value * projectileRandomization - projectileRandomization / 2
        );
    }
}
