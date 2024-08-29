using UnityEngine;
using TMPro;

public class TurnCounter : MonoBehaviour {

    public static TurnCounter instance;
    public static int CurrentTurn => instance.currentTurn;

    public TMP_Text counterText;

    int currentTurn = 0;
    int turnLimit;
    int turnsRemaining => turnLimit - currentTurn;
    
    public void SetTurnLimit(int value) {
        turnLimit = value;
        UpdateUI();
    }

    void Awake() => instance = this;

    void Start() {
        GameEvents.On(this, "player_turn_start", Increment);
        UpdateUI();
    }
    
    void OnDestroy() => GameEvents.RemoveListener(this, "player_turn_start");

    void Increment() {
        currentTurn++;
        if (turnsRemaining <= 0) {
            turnLimit = System.Math.Max(turnLimit - 1, 1);
            currentTurn = 0;
            GameEvents.Trigger("threat_increased");
        }
        UpdateUI();
    }

    void UpdateUI() {
        counterText.text = $"turns remaining: {turnsRemaining}";
    }
}