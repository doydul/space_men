using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ExplosionGenerator {

    private IIterableGrid grid;

    public ExplosionGenerator(IIterableGrid grid) {
        this.grid = grid;
    }

    public ExplosionCoverage Generate(Vector2 gridLocation, float blastSize) {
        var result = new ExplosionCoverage();

        var iterator = new LayeredGridIterator(grid, gridLocation);
        foreach (var layer in iterator.Layers()) {
            float blastWave = Mathf.Min(blastSize / layer.Count, 1);
            foreach (var square in layer) {
                result.AddSquare(square, blastWave);
            }
            blastSize -= layer.Count;
            if (blastSize <= 0) break;
        }
        return result;
    }
}
