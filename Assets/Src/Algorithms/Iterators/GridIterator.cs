using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class GridIterator {

    private IIterableGrid grid;
    private Vector2 start;

    public GridIterator(IIterableGrid grid, Vector2 start) {
        this.grid = grid;
        this.start = start;
    }

    public IEnumerable<Vector2> Squares() {
        return GraphNodes().Select(node => node.gridLocation);
    }

    public IEnumerable<GraphNode> GraphNodes() {
        if (!grid.ShouldIterate(start)) yield break;

        var leafNodes = new List<GraphNode>();
        var processedTiles = new List<Vector2>();
        leafNodes.Add(new GraphNode(start, null, 0));
        processedTiles.Add(start);
        while (leafNodes.Count > 0) {
            var newLeafNodes = new List<GraphNode>();
            foreach (var leafNode in leafNodes) {
                yield return leafNode;
                foreach (Vector2 adjTile in AdjacentLocations(leafNode.gridLocation)) {
                    if (grid.ShouldIterate(adjTile) && !processedTiles.Contains(adjTile)) {
                        newLeafNodes.Add(new GraphNode(adjTile, leafNode, leafNode.distance + 1));
                        processedTiles.Add(adjTile);
                    }
                }
            }
            leafNodes = newLeafNodes;
        }
    }

    private IEnumerable AdjacentLocations(Vector2 gridLocation) {
        yield return new Vector2(gridLocation.x - 1, gridLocation.y);
        yield return new Vector2(gridLocation.x + 1, gridLocation.y);
        yield return new Vector2(gridLocation.x, gridLocation.y - 1);
        yield return new Vector2(gridLocation.x, gridLocation.y + 1);
    }

    public class GraphNode {

      public Vector2 gridLocation { get; private set; }
      public GraphNode previousNode { get; private set; }
      public int distance { get; private set; }

      public GraphNode(Vector2 gridLocation, GraphNode previousNode, int distance) {
        this.gridLocation = gridLocation;
        this.previousNode = previousNode;
        this.distance = distance;
      }

      public IEnumerable<Vector2> Path() {
        var activeNode = this;
        while(activeNode != null) {
            yield return activeNode.gridLocation;
            activeNode = activeNode.previousNode;
        }
      }

      public IEnumerable<GraphNode> Nodes() {
          var activeNode = this;
        while(activeNode != null) {
            yield return activeNode;
            activeNode = activeNode.previousNode;
        }
      }
    }
}
