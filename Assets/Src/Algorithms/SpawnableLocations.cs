using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SpawnableLocations {

    private ISpawnable spawnable;

    public SpawnableLocations(ISpawnable spawnable) {
        this.spawnable = spawnable;
    }

    public List<Vector2> GetLocationsNear(Vector2 gridLocation, int count) {
        var result = new List<Vector2>();

        if (spawnable.SpawnableLocation(gridLocation)) {
            result.Add(gridLocation);
            count--;
        }

        var leafTiles = new List<Vector2>();
        var processedTiles = new List<Vector2>();
        leafTiles.Add(gridLocation);
        processedTiles.Add(gridLocation);
        while (count > 0) {
            var leaftTilesClone = new List<Vector2>(leafTiles);
            foreach (var leafTile in leaftTilesClone) {
                foreach (Vector2 adjTile in AdjacentLocations(leafTile)) {
                    if (!spawnable.WallLocation(adjTile) && !processedTiles.Contains(adjTile)) {
                        leafTiles.Add(adjTile);
                        processedTiles.Add(adjTile);
                    }
                }
                leafTiles.Remove(leafTile);
            }

            foreach (var leafTile in leafTiles) {
                if (spawnable.SpawnableLocation(leafTile)) {
                    result.Add(leafTile);
                    count--;
                    if (count <= 0) break;
                }
            }
            if (leafTiles.Count <= 0) break;
        }
        return result;
    }

    public bool SpawnableLocation(Vector2 gridLocation) {
        return spawnable.SpawnableLocation(gridLocation);
    }

    private IEnumerable AdjacentLocations(Vector2 gridLocation) {
        yield return new Vector2(gridLocation.x - 1, gridLocation.y);
        yield return new Vector2(gridLocation.x + 1, gridLocation.y);
        yield return new Vector2(gridLocation.x, gridLocation.y - 1);
        yield return new Vector2(gridLocation.x, gridLocation.y + 1);
    }
}
