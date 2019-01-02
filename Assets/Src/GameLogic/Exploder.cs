using UnityEngine;
using System.Collections.Generic;

public class Exploder {

    public Exploder(IWorld world) {
        this.world = world;
    }

    IWorld world;

    public Explosion PerformExplosion(ExploderInput profile) {
        var explosionGenerator = new ExplosionGenerator(new ExplodableWrapper(world));
        var explosion = explosionGenerator.Generate(profile.gridLocation, profile.blastRadius);
        var output = new Explosion(world.GetTileAt(profile.gridLocation));

        foreach (var square in explosion.squares) {
            output.AddExplodedTile(world.GetTileAt(square.gridLocation), square.blastCoverage);

            var targetSoldier = world.GetActorAt<Soldier>(square.gridLocation);
            var targetAlien = world.GetActorAt<Alien>(square.gridLocation);
            if (targetSoldier != null) {
                // TODO
            } else if (targetAlien != null) {
                if (Random.value <= square.blastCoverage) {
                    if (Random.value * 100 > targetAlien.armour - profile.armourPen) {
                        int damage = Random.Range(profile.minDamage, profile.maxDamage + 1);
                        targetAlien.Hurt(damage);
                        world.MakeBloodSplat(targetAlien);
                        output.AddCombatInstance(
                            new CombatInstance(targetAlien, CombatInstance.Status.Hit, damage)
                        );
                    } else {
                        output.AddCombatInstance(
                            new CombatInstance(targetAlien, CombatInstance.Status.Deflected)
                        );
                    }
                } else {
                    output.AddCombatInstance(
                        new CombatInstance(targetAlien, CombatInstance.Status.Missed)
                    );
                }
            }
        }
        return output;
    }

    public struct ExploderInput {

        public Vector2 gridLocation;
        public float blastRadius;
        public int minDamage;
        public int maxDamage;
        public int armourPen;
    }
}
