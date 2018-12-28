using UnityEngine;
using System.Linq;

public class ShootAction : ActionImpl, GameActions.ISoldierShootAction {

    public ShootAction(IWorld world, Exploder exploder) {
        this.world = world;
        this.exploder = exploder;
    }

    IWorld world;
    Exploder exploder;

    public DelayedAction Perform(Soldier shooter, Alien target) {
        shooter.ExpendAmmo();

        if (shooter.blast > 0) {
            ShootOrdnance(shooter, target);
        } else {
            ShootNormal(shooter, target);
        }
        return DelayedAction.Resolve();
    }

    void ShootNormal(Soldier shooter, Alien target) {
        bool hit = false;
        if (Random.value * 100 < shooter.accuracy + target.accModifier) {
            hit = true;
            if (Random.value * 100 > target.armour - shooter.armourPen) {
                int damage = Random.Range(shooter.minDamage, shooter.maxDamage + 1);
                target.ShowHitIndicator();
                target.Hurt(damage);
                world.MakeBloodSplat(target);
            } else {
                target.ShowDeflectIndicator();
            }
        }
        animationReel.PlayStandardShootAnimation(
            shooter: shooter,
            target: target.tile,
            type: hit ? ShootingAnimationType.Hit : ShootingAnimationType.Missed
        );
    }

    void ShootOrdnance(Soldier shooter, Alien target) {
        var gridLocation = target.gridLocation;
        if (Random.value * 100 > shooter.accuracy) {
            gridLocation = ScatterOrdnance(gridLocation);
        }
        exploder.PerformExplosion(new Exploder.ExplosionProfile() {
            gridLocation = target.gridLocation,
            blastRadius = shooter.blast,
            minDamage = shooter.minDamage,
            maxDamage = shooter.maxDamage,
            armourPen = shooter.armourPen
        });
    }

    Vector2 ScatterOrdnance(Vector2 gridLocation) {
        int scatterDistance = Random.value < 0.5f ? 1 : 2;
        var gridIterator = new NthLayerGridIterator(new ExplodableWrapper(world));
        var possibleScatterLocations = gridIterator.Squares(gridLocation, scatterDistance).ToList();
        return possibleScatterLocations[Random.Range(0, possibleScatterLocations.Count)];
    }
}
