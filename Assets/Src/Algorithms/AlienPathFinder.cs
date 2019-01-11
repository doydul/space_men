using UnityEngine;

public class AlienPathFinder {

    IAlienGrid grid;

    public AlienPathFinder(IAlienGrid grid) {
        this.grid = grid;
    }

    public Output ClosestTargetLocation(Vector2 start, int movement) {
        if (grid.IsTargetLocation(start)) return new Output() {targetLocation = start};
        var iterator = new GridIterator(grid, start);

        GridIterator.GraphNode invalidTargetNode = null;

        foreach (var node in iterator.GraphNodes()) {
            float distance = 0;
            if (invalidTargetNode != null) {
                distance = Mathf.Abs(invalidTargetNode.gridLocation.x - node.gridLocation.x) + Mathf.Abs(invalidTargetNode.gridLocation.y - node.gridLocation.y);
            }

            if (grid.IsTargetLocation(node.gridLocation)) {
                if (grid.IsValidFinishLocation(node.gridLocation) && distance <= 6) {
                    return GetOutput(node, movement);
                } else {
                    if (invalidTargetNode == null) {
                        invalidTargetNode = node;
                    }
                }
            }
        }

        if (invalidTargetNode != null) {
            foreach (var node in invalidTargetNode.Nodes()) {
                if (grid.IsValidFinishLocation(node.gridLocation)) {
                    return GetOutput(node, movement);
                }
            }
        }
        return new Output() {targetLocation = start};
    }

    Output GetOutput(GridIterator.GraphNode node, int movement) {
        var resultNode = node;
        foreach (var childNode in node.Nodes()) {
            resultNode = childNode;
            if (resultNode.distance <= movement) break;
        }
        return new Output() {
            targetLocation = resultNode.gridLocation,
            facing = resultNode.gridLocation - resultNode.previousNode.gridLocation
        };
    }

    public struct Output {
        public Vector2 targetLocation;
        public Vector2 facing;
    }
}
