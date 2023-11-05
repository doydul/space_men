using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public partial class Map {

    public int ManhattanDistance(Vector2 start, Vector2 end) {
        return (int)Mathf.Abs(start.x - end.x) + (int)Mathf.Abs(start.y - end.y);
    }

    public Path ShortestPath(IMask impassableTiles, Vector2 pointA, Vector2 pointB, bool stopWhenAdjacent = false) {
        return ShortestPath(impassableTiles, pointA, new Vector2[] { pointB }, stopWhenAdjacent);
    }

    public Path ShortestPath(IMask impassableTiles, Vector2 pointA, Vector2[] targetPoints, bool stopWhenAdjacent = false) {
        var root = new Node() { tile = GetTileAt(pointA) };
        var leafNodes = new PriorityQueue<Node, float>();
        var alreadyTraversed = new HashSet<Vector2>();
        var targetDistance = stopWhenAdjacent ? 1 : 0;
        leafNodes.Enqueue(root, 0);
        while(leafNodes.Count > 0) {
            var currentNode = leafNodes.Dequeue();
            alreadyTraversed.Add(currentNode.gridLocation);
            foreach (var adjacentTile in AdjacentTiles(currentNode.gridLocation)) {
                if (alreadyTraversed.Contains(adjacentTile.gridLocation)) continue;
                if (impassableTiles.Contains(new Point((int)adjacentTile.gridLocation.x, (int)adjacentTile.gridLocation.y))) continue;
                int shortestDistance = 99999;
                foreach (var targetPoint in targetPoints) {
                    var dist = ManhattanDistance(adjacentTile.gridLocation, targetPoint);
                    if (dist < shortestDistance) shortestDistance = dist;
                }
                var newNode = new Node() {
                    cumulativeWeight = currentNode.cumulativeWeight + 1,
                    heuristicWeight = shortestDistance * 1.1f,
                    tile = adjacentTile,
                    previous = currentNode
                };
                if (shortestDistance <= targetDistance) return Path.FromNode(newNode);
                leafNodes.Enqueue(newNode, newNode.totalWeight);
            }
        }
        return new Path();
    }

    public class Node {

        public Node previous;
        public float cumulativeWeight;
        public float heuristicWeight;
        public float totalWeight => cumulativeWeight + heuristicWeight;
        public Tile tile;
        public Vector2 gridLocation => tile.gridLocation;
    }

    public class Path {

        public Node[] nodes { get; private set; }
        public bool exists => nodes != null && nodes.Length > 0;
        public int length => nodes.Length;
        
        public static Path FromNode(Node node) {
            var nodes = new List<Node>();
            var currentNode = node;
            while (currentNode != null) {
                nodes.Add(currentNode);
                currentNode = currentNode.previous;
            }
            nodes.Reverse();
            var result = new Path();
            result.nodes = nodes.ToArray();
            return result;
        }
    }
}