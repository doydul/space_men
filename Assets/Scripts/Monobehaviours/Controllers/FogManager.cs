using UnityEngine;

public class FogManager : MonoBehaviour {
    
    public static FogManager instance;
    void Awake() => instance = this;
    
    public static bool disabled;

    void Start() => GameEvents.On(this, "player_turn_start", () => UpdateFog(true));
    void OnDestroy() => GameEvents.RemoveListener(this, "player_turn_start");
    
    public void Disable() => disabled = true;

    public void UpdateFog(bool reset = false) {
        var soldiers = Map.instance.GetActors<Soldier>();
        foreach (var tile in Map.instance.EnumerateTiles()) {
            if (disabled) {
                tile.RemoveFoggy();
                continue;
            }
            if (!tile.open) continue;
            if (reset) tile.SetFoggy();
            foreach (var soldier in soldiers) {
                if (Map.instance.ManhattanDistance(tile.gridLocation, soldier.gridLocation) < soldier.sightRange && soldier.CanSee(tile.gridLocation)) {
                    tile.RemoveFoggy();
                    
                    // tutorials
                    if (tile.HasActor<Chest>()) Tutorial.Show(tile.transform, "crates", true);
                    //
                    
                    break;
                }
            }
        }
    }
}