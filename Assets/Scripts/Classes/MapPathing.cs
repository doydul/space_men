using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public partial class Map {

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
        public int length => nodes == null ? 0 : nodes.Length - 1;
        public Node last => nodes[nodes.Length - 1];
        public Node penultimate => nodes[nodes.Length - 2];
        
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

    public int ManhattanDistance(Vector2 start, Vector2 end) {
        return (int)Mathf.Abs(start.x - end.x) + (int)Mathf.Abs(start.y - end.y);
    }

    public Path ShortestPath(IMask impassableTiles, Vector2 pointA, Vector2 pointB, bool stopWhenAdjacent = false) {
        return ShortestPath(impassableTiles, pointA, new Vector2[] { pointB }, stopWhenAdjacent);
    }

    public Path ShortestPath(IMask impassableTiles, Vector2 pointA, Vector2[] targetPoints, bool stopWhenAdjacent = false) {
        var root = new Node() { tile = GetTileAt(pointA) };
        var targetDistance = stopWhenAdjacent ? 1 : 0;
        foreach (var targetPoint in targetPoints) {
            if (ManhattanDistance(pointA, targetPoint) <= targetDistance) return Path.FromNode(root);
        }
        var leafNodes = new PriorityQueue<Node, float>();
        var alreadyTraversed = new HashSet<Vector2>();
        leafNodes.Enqueue(root, 0);
        int i = 0;
        while(leafNodes.Count > 0) {
            i++;
            if (i > 10000) {
                Debug.LogError("Infinite loop detected during path finding!");
                Debug.Log(impassableTiles);
                Debug.Log(pointA);
                Debug.Log(targetPoints.Length);
                Debug.Log(stopWhenAdjacent);
                break;
            }
            var currentNode = leafNodes.Dequeue();
            alreadyTraversed.Add(currentNode.gridLocation);
            foreach (var adjacentTile in AdjacentTiles(currentNode.gridLocation)) {
                if (alreadyTraversed.Contains(adjacentTile.gridLocation)) continue;
                if (impassableTiles.Contains(adjacentTile)) continue;
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

    public bool RayOverlapsTile(Vector2 rayStart, Vector2 rayEnd, Vector2 tilePos) {
        var rayDelta = rayEnd - rayStart;
        var tileDelta = tilePos - rayStart;
        if (rayDelta.x == 0) return tileDelta.x == 0;
        if (rayDelta.y == 0) return tileDelta.y == 0;
        float c1 = (rayDelta.y / rayDelta.x) * (tileDelta.x - 0.5f);
        float c2 = (rayDelta.y / rayDelta.x) * (tileDelta.x + 0.5f);
        float c3 = (rayDelta.x / rayDelta.y) * (tileDelta.y - 0.5f);
        float c4 = (rayDelta.x / rayDelta.y) * (tileDelta.y + 0.5f);
        float b1 = (rayDelta.y / rayDelta.x) * (tileDelta.x);
        float b2 = (rayDelta.x / rayDelta.y) * (tileDelta.y);
        return Mathf.Abs(c1 - tileDelta.y) < 0.49f || Mathf.Abs(c2 - tileDelta.y) < 0.49f || Mathf.Abs(c3 - tileDelta.x) < 0.49f || Mathf.Abs(c4 - tileDelta.x) < 0.49f || (Mathf.Abs(b1 - tileDelta.y) < 0.01f && Mathf.Abs(b2 - tileDelta.x) < 0.01f);
    }

    public bool CanBeSeenFrom(IMask opaque, Vector2 pointA, Vector2 pointB) {
        var bottomCorner = new Vector2(pointA.x < pointB.x ? pointA.x : pointB.x, pointA.y < pointB.y ? pointA.y : pointB.y);
        var topCorner = new Vector2(pointA.x > pointB.x ? pointA.x : pointB.x, pointA.y > pointB.y ? pointA.y : pointB.y);
        var delta = topCorner - bottomCorner;
        for (int x = 0; x <= delta.x; x++) {
            for (int y = 0; y <= delta.y; y++) {
                var tilePos = bottomCorner + new Vector2(x, y);
                if (RayOverlapsTile(pointA, pointB, tilePos) && opaque.Contains(GetTileAt(tilePos))) return false;
            }
        }
        return true;
    }
}