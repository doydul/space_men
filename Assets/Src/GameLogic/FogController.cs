using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;

public class FogController : MonoBehaviour {

    private const int FOG_RADIUS = 8;

    public Map map;
    public Commander commander;
    public UnityEvent FogChanged;

    void Awake() {
        if (FogChanged == null) FogChanged = new UnityEvent();
        commander.PlayerMoved.AddListener(Recalculate);
    }

    void Start() {
        Recalculate();
    }

    private void Recalculate() {
        var targets = map.GetActors<Soldier>().Select(soldier => soldier.gridLocation).ToList();
        var fogGrid = new FogGrid(targets, FOG_RADIUS);
        foreach(Tile tile in map.EnumerateTiles()) {
            if (fogGrid.InFog(tile.gridLocation)) {
                tile.SetFoggy();
            } else {
                tile.RemoveFoggy();
            }
        }
        FogChanged.Invoke();
    }
}
