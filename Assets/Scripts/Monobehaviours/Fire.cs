using UnityEngine;

public class Fire : MonoBehaviour {
    
    public Tile tile;

    public int minDamage;
    public int maxDamage;
    public int timer;

    void Start() {
        GameEvents.On(this, "player_turn_start", UpdateCounter);
    }

    void OnDestroy() {
        GameEvents.RemoveListener(this, "player_turn_start");
    }

    private void UpdateCounter() {
        timer--;
        if (timer <= 0) {
            Destroy(gameObject);
        }
    }
}