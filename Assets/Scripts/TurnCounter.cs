using UnityEngine;
using TMPro;

public class TurnCounter : MonoBehaviour {

    public static TurnCounter instance;
    public static int CurrentTurn => instance.currentTurn;

    public TMP_Text counterText;

    int currentTurn = 1;

    void Awake() {
        instance = this;
    }

    void Start() {
        GameEvents.On(this, "player_turn_start", Increment);
        UpdateUI();
    }

    void Increment() {
        currentTurn++;
        UpdateUI();
    }

    void UpdateUI() {
        counterText.text = $"Current Turn: {currentTurn}";
    }
}