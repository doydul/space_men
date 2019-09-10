using UnityEngine;
using System.Linq;

public class MapStore : IMapStore {

    public Map map { private get; set; }

    public Data.Cell[,] GetMap() {
        var result = new Data.Cell[map.width, map.height];
        for (int i = 0; i < map.width; i++) {
            for (int j = 0; j < map.height; j++) {
                var gridLocation = new Vector2(i, j);
                var tile = map.GetTileAt(gridLocation);
                var mapCell = new Data.Cell();
                mapCell.isWall = !tile.open;
                mapCell.position = new Data.Position {
                    x = i,
                    y = j
                };
                mapCell.isSpawnPoint = map.startLocations.Any(startLocation => (int)startLocation.gridLocation.x == i && (int)startLocation.gridLocation.y == j);
                mapCell.isAlienSpawnPoint = map.spawners.Any(spawner => (int)spawner.gridLocation.x == i && (int)spawner.gridLocation.y == j);
                result[i, j] = mapCell;
            }
        }
        return result;
    }
}
