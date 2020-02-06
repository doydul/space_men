using System.Linq;

public class FogController {

    public static FogController instance { get; private set; }

    public FogController(IGameMap map, IGameEvent fogChanged) {
        instance = this;
    }
}
