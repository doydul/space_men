using UnityEngine;
using System.Collections.Generic;

public class AdjacentSquaresGridIterator {

    IIterableGrid grid;

    public AdjacentSquaresGridIterator(IIterableGrid grid) {
        this.grid = grid;
    }

    public IEnumerable<Vector2> Squares(Vector2 start) {
        foreach (var square in AdjacentLocations(start)) {
            if (grid.ShouldIterate(square)) yield return square;
        }
    }

    IEnumerable<Vector2> AdjacentLocations(Vector2 gridLocation) {
        yield return new Vector2(gridLocation.x - 1, gridLocation.y);
        yield return new Vector2(gridLocation.x + 1, gridLocation.y);
        yield return new Vector2(gridLocation.x, gridLocation.y - 1);
        yield return new Vector2(gridLocation.x, gridLocation.y + 1);
    }
}
