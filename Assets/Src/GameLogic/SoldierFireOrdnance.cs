using UnityEngine;
using System.Linq;

public static class SoldierFireOrdnance {

    public static void Execute(Soldier soldier, Tile target, IWorld world) {
        soldier.ExpendAmmo();

        var gridLocation = target.gridLocation;
        if (Random.value * 100 > soldier.accuracy) {
            gridLocation = Scatter(gridLocation, world);
        }
        ExecuteExplosion(gridLocation, soldier, world);
    }

    private static void ExecuteExplosion(Vector2 gridLocation, Soldier soldier, IWorld world) {
        var explosionGenerator = new ExplosionGenerator(new ExplodableWrapper(world));
        var explosion = explosionGenerator.Generate(gridLocation, soldier.blast);

        foreach (var square in explosion.squares) {
            world.CreateExplosionMarker(square.gridLocation, square.blastCoverage);

            var targetSoldier = world.GetActorAt<Soldier>(square.gridLocation);
            var targetAlien = world.GetActorAt<Alien>(square.gridLocation);
            if (targetSoldier != null) {
                // TODO
            } else if (targetAlien != null) {
                if (Random.value <= square.blastCoverage) {
                    if (Random.value * 100 > targetAlien.armour - soldier.armourPen) {
                        int damage = Random.Range(soldier.minDamage, soldier.maxDamage + 1);
                        targetAlien.ShowHitIndicator();
                        targetAlien.Hurt(damage);
                        world.MakeBloodSplat(targetAlien);
                    } else {
                        targetAlien.ShowDeflectIndicator();
                    }
                }
            }
        }
    }

    private static Vector2 Scatter(Vector2 gridLocation, IWorld world) {
        int scatterDistance = Random.value < 0.5f ? 1 : 2;
        var gridIterator = new NthLayerGridIterator(new ExplodableWrapper(world));
        var possibleScatterLocations = gridIterator.Squares(gridLocation, scatterDistance).ToList();
        return possibleScatterLocations[Random.Range(0, possibleScatterLocations.Count)];
    }
}
