using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class RadiusGridIterator {

    IIterableGrid grid;
    Vector2 start;
    int radius;

    public RadiusGridIterator(IIterableGrid grid, Vector2 start, int radius) {
        this.grid = grid;
        this.start = start;
        this.radius = radius;
    }

    public IEnumerable<Vector2> Squares() {
        var gridIterator = new GridIterator(grid, start);
        foreach (var square in gridIterator.Squares()) {
            int distance = (int)((Mathf.Abs(start.x - square.x) + Mathf.Abs(start.y - square.y)));
            if (distance <= radius) {
                yield return square;
            } else {
                break;
            }
        }
    }
}
