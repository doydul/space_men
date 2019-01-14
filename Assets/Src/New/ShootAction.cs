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
        ShootingAnimationType type = ShootingAnimationType.Missed;
        if (Random.value * 100 < shooter.accuracy + target.accModifier) {
            type = ShootingAnimationType.Deflected;
            if (Random.value * 100 > target.armour - shooter.armourPen) {
                type = ShootingAnimationType.Hit;
                int damage = Random.Range(shooter.minDamage, shooter.maxDamage + 1);
                target.Hurt(damage);
                world.MakeBloodSplat(target);
            }
        }
        if (target.dead) shooter.GetExp(target.expReward);
        animationReel.PlayStandardShootAnimation(
            shooter: shooter,
            target: target.tile,
            type: type
        );
    }

    void ShootOrdnance(Soldier shooter, Alien target) {
        var gridLocation = target.gridLocation;
        if (Random.value * 100 > shooter.accuracy) {
            gridLocation = ScatterOrdnance(gridLocation);
        }
        var explosion = exploder.PerformExplosion(new Exploder.ExploderInput() {
            gridLocation = gridLocation,
            blastRadius = shooter.blast,
            minDamage = shooter.minDamage,
            maxDamage = shooter.maxDamage,
            armourPen = shooter.armourPen
        });
        foreach (var actor in explosion.combatInstances.Select(instance => instance.target)) {
            var alien = (Alien)actor;
            if (alien != null) {
                if (alien.dead) shooter.GetExp(alien.expReward);
            }
        }
        animationReel.PlayOrdnanceShootAnimation(
            shooter: shooter,
            explosion: explosion
        );
    }

    Vector2 ScatterOrdnance(Vector2 gridLocation) {
        int scatterDistance = Random.value < 0.5f ? 1 : 2;
        var gridIterator = new NthLayerGridIterator(new ExplodableWrapper(world));
        var possibleScatterLocations = gridIterator.Squares(gridLocation, scatterDistance).ToList();
        return possibleScatterLocations[Random.Range(0, possibleScatterLocations.Count)];
    }
}
