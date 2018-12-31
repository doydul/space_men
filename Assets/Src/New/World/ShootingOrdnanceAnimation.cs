using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShootingOrdnanceAnimation : WorldAnimation {

    Soldier shooter;
    Exploder.ExploderOutput exploderOutput;

    List<GameObject> explosionClouds;

    public ShootingOrdnanceAnimation(
        Soldier shooter,
        Exploder.ExploderOutput exploderOutput
    ) {
        this.shooter = shooter;
        this.exploderOutput = exploderOutput;
    }

    public override IEnumerator Animate(WorldAnimation.IAnimationInteractor interactor, DelayedAction delayedAction) {
        var gunflareObject = interactor.MakeGunflare(shooter.muzzlePosition, shooter.rotation);
        yield return new WaitForSeconds(0.1f);
        var tracer = interactor.MakeTracer(shooter.muzzlePosition);
        yield return tracer.StartAnimating(shooter.muzzlePosition, exploderOutput.centreTile.realLocation, 1);
        tracer.Destroy();
        Object.Destroy(gunflareObject);
        MakeExplosionClouds(interactor);
        yield return new WaitForSeconds(1f);
        DestroyExplosionClouds();
        delayedAction.Finish();
    }

    void MakeExplosionClouds(WorldAnimation.IAnimationInteractor interactor) {
        explosionClouds = new List<GameObject>();
        foreach (var explodedTile in exploderOutput.explodedTiles) {
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
