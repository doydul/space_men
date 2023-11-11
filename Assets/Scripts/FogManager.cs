using UnityEngine;

public class FogManager : MonoBehaviour {
    
    public static FogManager instance;

    void Awake() {
        instance = this;
    }

    void Start() {
        GameEvents.On(this, "player_turn_start", () => UpdateFog(true));
    }

    void OnDestroy() {
        GameEvents.RemoveListener(this, "player_turn_start");
    }

    public void UpdateFog(bool reset = false) {
        var soldiers = Map.instance.GetActors<Soldier>();
        foreach (var tile in Map.instance.EnumerateTiles()) {
            if (!tile.open) continue;
            if (reset) tile.SetFoggy();
            foreach (var soldier in soldiers) {
                if (Map.instance.ManhattanDistance(tile.gridLocation, soldier.gridLocation) < soldier.sightRange && soldier.CanSee(tile.gridLocation)) {
                    tile.RemoveFoggy();
                    break;
                }
            }
        }
    }
}