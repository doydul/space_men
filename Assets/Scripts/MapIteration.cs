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

        public IEnumerable<Tile> EnumerateFrom(Vector2 startLocation) {
            var alreadyTraversed = new HashSet<Vector2>();
            var leafTiles = new Queue<Tile>();
            leafTiles.Enqueue(map.GetTileAt(startLocation));
            while (leafTiles.Count > 0) {
                var currentTile = leafTiles.Dequeue();
                yield return currentTile;
                foreach (var tile in map.AdjacentTiles(currentTile)) {
                    if (alreadyTraversed.Contains(tile.gridLocation)) continue;
                    if (exclusionMask != null && exclusionMask.Contains(tile)) continue;
                    leafTiles.Enqueue(tile);
                }
            }
        }

        public IEnumerable<Tile> RadiallyFrom(Vector2 startLocation, int radius) {
            foreach (var tile in EnumerateFrom(startLocation)) {
                if (map.ManhattanDistance(startLocation, tile.gridLocation) <= radius) {
                    yield return tile;
                } else {
                    break;
                }
            }
        }
    }
}