using UnityEngine;

public class Exploder {

    public Exploder(IWorld world) {
        this.world = world;
    }

    IWorld world;

    public void PerformExplosion(ExplosionProfile profile) {
        var explosionGenerator = new ExplosionGenerator(new ExplodableWrapper(world));
        var explosion = explosionGenerator.Generate(profile.gridLocation, profile.blastRadius);

        foreach (var square in explosion.squares) {
            world.CreateExplosionMarker(square.gridLocation, square.blastCoverage);

            var targetSoldier = world.GetActorAt<Soldier>(square.gridLocation);
            var targetAlien = world.GetActorAt<Alien>(square.gridLocation);
            if (targetSoldier != null) {
                // TODO
            } else if (targetAlien != null) {
                if (Random.value <= square.blastCoverage) {
                    if (Random.value * 100 > targetAlien.armour - profile.armourPen) {
                        int damage = Random.Range(profile.minDamage, profile.maxDamage + 1);
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

    public struct ExplosionProfile {

        public Vector2 gridLocation;
        public float blastRadius;
        public int minDamage;
        public int maxDamage;
        public int armourPen;
    }
}
