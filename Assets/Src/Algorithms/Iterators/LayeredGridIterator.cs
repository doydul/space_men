using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class LayeredGridIterator {

    private IIterableGrid grid;
    private Vector2 start;

    public LayeredGridIterator(IIterableGrid grid, Vector2 start) {
        this.grid = grid;
        this.start = start;
    }

    public IEnumerable<List<Vector2>> Layers() {
        var gridIterator = new GridIterator(grid, start);
        var layer = new List<Vector2>();
        int distance = 0;
        foreach (var square in gridIterator.Squares()) {
            int squareDistance = (int)(Mathf.Abs(square.x - start.x) + Mathf.Abs(square.y - start.y));
            if (squareDistance == distance + 1) {
                yield return layer;
                layer = new List<Vector2>();
                distance++;
            } else if (distance != squareDistance) {
                throw new System.Exception("Iterator error");
            }
            layer.Add(square);
        }
        if (layer.Count > 0) yield return layer;
    }
}
