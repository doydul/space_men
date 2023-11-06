using UnityEngine;

public class InterfaceController : MonoBehaviour {
    
    public void EndTurn() {
        UIState.instance.EndPlayerTurn();
        GameEvents.Trigger("alien_turn_start");
    }
}