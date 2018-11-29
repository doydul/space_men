using System.Linq;
using System.Collections.Generic;

public class GameMapImpl : IGameMap {

    public GameMapImpl(Map map) {
        this.map = map;
    }

    Map map;

    public List<Soldier> soldiers { get { return map.GetActors<Soldier>(); } }

    public List<Tile> tiles { get { return map.EnumerateTiles().ToList(); } }
}
