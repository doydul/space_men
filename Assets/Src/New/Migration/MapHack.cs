using UnityEngine;

public static class MapHack {
    
    static Map map;
    
    public static void Init(Map map) {
        MapHack.map = map;
    }
    
    public static Data.Map GetData() {
        var result = new Data.Map();
        result.cells = new Data.Cell[map.width, map.height];
        for (int i = 0; i < map.width; i++) {
            for (int j = 0; j < map.height; j++) {
                var gridLocation = new Vector2(i, j);
                var tile = map.GetTileAt(gridLocation);
                var mapSquare = new Data.Cell();
                mapSquare.isWall = !tile.open;
                mapSquare.isFoggy = tile.foggy;
                if (map.GetActorAt<Soldier>(gridLocation) != null) {
                    mapSquare.actorType = Data.ActorType.Soldier;
                    mapSquare.soldierIndex = map.GetActorAt<Soldier>(gridLocation).index;
                } else if (map.GetActorAt<Alien>(gridLocation) != null) {
                    mapSquare.actorType = Data.ActorType.Alien;
                } else {
                    mapSquare.actorType = Data.ActorType.None;
                }
                mapSquare.position = new Data.Position {
                    x = i,
                    y = j
                };
                result.cells[i, j] = mapSquare;
            }
        }
        return result;
    }
}
