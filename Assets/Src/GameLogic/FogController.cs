using System.Linq;

public class FogController {

    private const int FOG_RADIUS = 6;

    public FogController(IGameMap map, IGameEvent fogChanged) {
        this.map = map;
        this.fogChanged = fogChanged;
    }

    IGameMap map;
    IGameEvent fogChanged;

    public void Recalculate() {
        var targets = map.soldiers.Select(soldier => soldier.gridLocation).ToList();
        var fogGrid = new FogGrid(targets, FOG_RADIUS);
        foreach(Tile tile in map.tiles) {
            if (fogGrid.InFog(tile.gridLocation)) {
                tile.SetFoggy();
            } else {
                tile.RemoveFoggy();
            }
        }
        fogChanged.Invoke();
    }
}
