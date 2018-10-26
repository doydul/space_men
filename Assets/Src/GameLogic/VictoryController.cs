using UnityEngine;

public class VictoryController : MonoBehaviour {

    public void Win() {
        GameLogicComponent.userInterface.ShowVictoryPopup();
    }
}
