using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PathFinder2 {

    public PathFinder2(IPathable2 grid) {
        this.grid = grid;
    }

    IPathable2 grid;

    public PathingNode ShortestPath(Vector2 start) {
        var firstNode = new PathingNode(start, null, 0, 0);
        if (grid.FinishLocation(start)) return firstNode;

        var traversedSquares = new HashSet<Vector2> { start };
        var leafNodes = new List<PathingNode> { firstNode };

        while (leafNodes.Count > 0) {
            var node = BestNode(leafNodes);
            leafNodes.Remove(node);

            var adjacent = new AdjacentSquaresGridIterator(
                new GridIterationAdaptor(grid)
            );
            foreach (var square in adjacent.Squares(node.square)) {
                if (traversedSquares.Contains(square)) continue;
                int heuristic = grid.HeuristicFor(square);
                var newNode = new PathingNode(square, node, node.pathLength + 1, heuristic);
                if (grid.FinishLocation(square)) return newNode;
                leafNodes.Add(newNode);
                traversedSquares.Add(square);
            }
        }
        return firstNode;
    }

    // Private

    PathingNode BestNode(List<PathingNode> nodes) {
        return nodes.Aggregate((curMin, node) => (curMin == null || node.weight < curMin.weight ? node : curMin));
    }
}

public interface IPathable2 {
    bool Pathable(Vector2 gridLocation);
    bool FinishLocation(Vector2 gridLocation);
    int HeuristicFor(Vector2 gridLocation);
}

public class GridIterationAdaptor : IIterableGrid {

    IPathable2 pathable;

    public GridIterationAdaptor(IPathable2 pathable) {
        this.pathable = pathable;
    }

    public bool ShouldIterate(Vector2 gridLocation) {
        return pathable.Pathable(gridLocation);
    }
}

public class PathingNode {

    public PathingNode(Vector2 square, PathingNode previousNode, int pathLength, int heuristic) {
      this.square = square;
      this.previousNode = previousNode;
      this.pathLength = pathLength;
      this.heuristic = heuristic;
    }

    public Vector2 square { get; private set; }
    public PathingNode previousNode { get; private set; }
    public int pathLength { get; private set; }
    public int heuristic { get; private set; }

    public int weight { get { return pathLength + heuristic; } }

    public IEnumerable<Vector2> path { get {
        var currentNode = this;
        while(currentNode != null) {
            yield return currentNode.square;
            currentNode = currentNode.previousNode;
        }
    } }
}

public class SoldierPathingWrapper2 : IPathable2 {

    public SoldierPathingWrapper2(Map map, Vector2 target) {
        this.map = map;
        this.target = target;
    }

    Map map;
    Vector2 target;

    public bool Pathable(Vector2 gridLocation) {
        var tile = map.GetTileAt(gridLocation);
        if (tile == null || tile.actor != null && tile.GetActor<Soldier>() == null) return false;
        return tile.open;
    }

    public bool FinishLocation(Vector2 gridLocation) {
        return gridLocation == target;
    }

    public int HeuristicFor(Vector2 gridLocation) {
        return (int)(Mathf.Abs(gridLocation.x - target.x) + Mathf.Abs(gridLocation.y - target.y));
    }
}
