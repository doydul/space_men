using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public partial class Map {

    public Iterator iterator => new Iterator(this);

    public class Iterator {

        Map map;
        IMask exclusionMask;

        public Iterator(Map map) {
            this.map = map;
        }

        public Iterator Exclude(IMask exclusionMask) {
            this.exclusionMask = exclusionMask;
            return this;
        }

        public IEnumerable<IterationNode> EnumerateFrom(Vector2 startLocation) {
            return EnumerateFrom(new Vector2[] { startLocation });
        }
        
        public IEnumerable<IterationNode> EnumerateFrom(IEnumerable<Vector2> startLocations) {
            var alreadyTraversed = new HashSet<Vector2>();
            var leafTiles = new Queue<IterationNode>();
            foreach (var startLocation in startLocations) {
                leafTiles.Enqueue(new IterationNode { tile = map.GetTileAt(startLocation) });
            }
            int i = 0;
            while (leafTiles.Count > 0) {
                i++;
                if (i > 10000) {
                    Debug.LogError("Infinite loop detected while iterating over tiles!");
                    break;
                }
                var currentTile = leafTiles.Dequeue();
                yield return currentTile;
                foreach (var tile in map.AdjacentTiles(currentTile.tile)) {
                    if (alreadyTraversed.Contains(tile.gridLocation)) continue;
                    if (exclusionMask != null && exclusionMask.Contains(tile)) continue;
                    leafTiles.Enqueue(new IterationNode { tile = tile, distanceFromOrigin = currentTile.distanceFromOrigin + 1, userData = currentTile.userData });
                    alreadyTraversed.Add(tile.gridLocation);
                }
            }
        }

        public IEnumerable<Tile> RadiallyFrom(Vector2 startLocation, int radius) {
            foreach (var tile in EnumerateFrom(startLocation)) {
                if (tile.distanceFromOrigin <= radius) {
                    yield return tile.tile;
                } else {
                    break;
                }
            }
        }

        public IEnumerable<List<Tile>> EnumerateLayersFrom(Vector2 startLocation) {
            var currentLayer = new List<Tile>();
            var currentLayerRadius = 0;
            foreach (var tile in EnumerateFrom(startLocation)) {
                if (tile.distanceFromOrigin > currentLayerRadius) {
                    yield return currentLayer;
                    currentLayer = new();
                    currentLayerRadius++;
                }
                currentLayer.Add(tile.tile);
            }
        }
    }
    
    
    public class IterationNode {

        public Tile tile;
        public int distanceFromOrigin;
        public int userData;
    }
    
    public List<Vector2> EvenlySpacedPoints(IMask mask, int count) {
        var result = new List<Vector2>();
        var viableTiles = EnumerateTiles().Where(tile => !mask.Contains(tile)).ToList();
        
        int minDist = (int)Mathf.Sqrt(2 *viableTiles.Count / count);
        int attempts = 0;
        
        while (attempts < 10) {
            var activeTile = viableTiles.SampleWithCount(viableTiles.Count);
            while (activeTile != null) {
                result.Add(activeTile.gridLocation);
                viableTiles = viableTiles.Where(tile => ManhattanDistance(activeTile.gridLocation, tile.gridLocation) >= minDist).ToList();
                if (viableTiles.Count > 0) activeTile = viableTiles.SampleWithCount(viableTiles.Count);
                else activeTile = null;
            }
            Debug.Log(result.Count);
            if (result.Count > count) {
                minDist += 1;
                result = new();
                viableTiles = EnumerateTiles().Where(tile => !mask.Contains(tile)).ToList();
            } else if (result.Count < count) {
                minDist -= 1;
                result = new();
                viableTiles = EnumerateTiles().Where(tile => !mask.Contains(tile)).ToList();
            } else {
                return result;
            }
            attempts++;
            Debug.Log($"attempt number {attempts}");
        }
        return result;
    }
}