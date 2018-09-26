using System.Collections.Generic;
using UnityEngine;

public class Explosion {

    public List<ExplosionSquare> squares { get; private set; }

    public Explosion() {
        squares = new List<ExplosionSquare>();
    }

    public void AddSquare(Vector2 gridLocation, float blastCoverage) {
        squares.Add(new ExplosionSquare(gridLocation, blastCoverage));
    }

    public class ExplosionSquare {

        public Vector2 gridLocation { get; private set; }
        public float blastCoverage { get; private set; }

        public ExplosionSquare(Vector2 gridLocation, float blastCoverage) {
            this.gridLocation = gridLocation;
            this.blastCoverage = blastCoverage;
        }
    }
}
