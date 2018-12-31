using UnityEngine;
using System.Collections.Generic;

public class Exploder {

    public Exploder(IWorld world) {
        this.world = world;
    }

    IWorld world;

    public ExploderOutput PerformExplosion(ExploderInput profile) {
        var explosionGenerator = new ExplosionGenerator(new ExplodableWrapper(world));
        var explosion = explosionGenerator.Generate(profile.gridLocation, profile.blastRadius);
        var output = new ExploderOutput(world.GetTileAt(profile.gridLocation));

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
                        targetAlien.ShowHitIndicator();
                        targetAlien.Hurt(damage);
                        world.MakeBloodSplat(targetAlien);
                        output.AddHurtActor(targetAlien);
                    } else {
                        targetAlien.ShowDeflectIndicator();
                    }
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

    public class ExploderOutput {

        public Tile centreTile { get; private set; }
        public List<ExplodedTile> explodedTiles { get; private set; }
        public List<Actor> hurtActors { get; private set; }

        public ExploderOutput(Tile centreTile) {
            this.centreTile = centreTile;
            explodedTiles = new List<ExplodedTile>();
            hurtActors = new List<Actor>();
        }

        public void AddHurtActor(Actor actor) {
            hurtActors.Add(actor);
        }

        public void AddExplodedTile(Tile tile, float blastCoverage) {
            explodedTiles.Add(new ExplodedTile(tile, blastCoverage));
        }

        public class ExplodedTile {

            public Tile tile { get; private set; }
            public float blastCoverage { get; private set; }

            public ExplodedTile(Tile tile, float blastCoverage) {
                this.tile = tile;
                this.blastCoverage = blastCoverage;
            }
        }
    }
}
