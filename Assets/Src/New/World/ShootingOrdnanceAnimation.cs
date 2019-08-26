using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShootingOrdnanceAnimation : WorldAnimation {

    Soldier shooter;
    Explosion explosion;

    List<GameObject> explosionClouds;

    public ShootingOrdnanceAnimation(
        Soldier shooter,
        Explosion explosion
    ) {
        this.shooter = shooter;
        this.explosion = explosion;
    }

    public override IEnumerator Animate(WorldAnimation.IAnimationInteractor interactor, DelayedAction delayedAction) {
        var gunflareObject = interactor.MakeGunflare(shooter.muzzlePosition, shooter.rotation);
        // yield return new WaitForSeconds(0.1f);
        // var tracer = interactor.MakeTracer(shooter.muzzlePosition, shooter.weaponName);
        // yield return tracer.StartAnimating(shooter.muzzlePosition, explosion.centreTile.realLocation);
        // tracer.Destroy();
        yield return new WaitForSeconds(0.5f);
        Object.Destroy(gunflareObject);
        MakeExplosionClouds(interactor);
        yield return new WaitForSeconds(1f);
        DestroyExplosionClouds();
        delayedAction.Finish();
    }

    void MakeExplosionClouds(WorldAnimation.IAnimationInteractor interactor) {
        explosionClouds = new List<GameObject>();
        foreach (var explodedTile in explosion.explodedTiles) {
            explosionClouds.Add(
                interactor.MakeExplosionCloud(
                    explodedTile.tile.realLocation
                )
            );
        }
    }

    void DestroyExplosionClouds() {
        foreach (var cloud in explosionClouds) {
            Object.Destroy(cloud);
        }
    }
}
