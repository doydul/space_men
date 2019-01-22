using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class ExplosionGenerator {

    private IIterableGrid grid;

    public ExplosionGenerator(IIterableGrid grid) {
        this.grid = grid;
    }

    public ExplosionCoverage Generate(Vector2 gridLocation, float blastSize) {
        var result = new ExplosionCoverage();

        var iterator = new LayeredGridIterator(grid, gridLocation);
        int layerI = -1;
        foreach (var layer in iterator.Layers()) {
            blastSize -= Mathf.Max(layerI, 0);
            foreach (var square in layer.OrderBy(x => Random.value).Take((int)blastSize)) {
                result.AddSquare(square, 1);
                blastSize--;
            }
            if (blastSize <= 0) break;
            layerI++;
        }
        return result;
    }
}
