using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class NthLayerGridIterator {

    private IIterableGrid grid;

    public NthLayerGridIterator(IIterableGrid grid) {
        this.grid = grid;
    }

    public IEnumerable<Vector2> Squares(Vector2 start, int n) {
        var gridIterator = new LayeredGridIterator(grid, start);
        int i = 0;
        foreach (var layer in gridIterator.Layers()) {
            if (i == n) {
                foreach (var square in layer) {
                    yield return square;
                }
                yield break;
            }
            i++;
        }
    }
}
