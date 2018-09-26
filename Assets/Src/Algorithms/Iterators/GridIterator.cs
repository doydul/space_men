using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GridIterator {

    private IIterableGrid grid;
    private Vector2 start;

    public GridIterator(IIterableGrid grid, Vector2 start) {
        this.grid = grid;
        this.start = start;
    }

    public IEnumerable<Vector2> Squares() {
        if (!grid.ShouldIterate(start)) {
            yield break;
        }

        var leafTiles = new List<Vector2>();
        var processedTiles = new List<Vector2>();
        leafTiles.Add(start);
        processedTiles.Add(start);
        while (leafTiles.Count > 0) {
            var newLeafTiles = new List<Vector2>();
            foreach (var leafTile in leafTiles) {
                yield return leafTile;
                foreach (Vector2 adjTile in AdjacentLocations(leafTile)) {
                    if (grid.ShouldIterate(adjTile) && !processedTiles.Contains(adjTile)) {
                        newLeafTiles.Add(adjTile);
                        processedTiles.Add(adjTile);
                    }
                }
            }
            leafTiles = newLeafTiles;
        }
    }

    private IEnumerable AdjacentLocations(Vector2 gridLocation) {
        yield return new Vector2(gridLocation.x - 1, gridLocation.y);
        yield return new Vector2(gridLocation.x + 1, gridLocation.y);
        yield return new Vector2(gridLocation.x, gridLocation.y - 1);
        yield return new Vector2(gridLocation.x, gridLocation.y + 1);
    }
}
