using UnityEngine;

public class Main : MonoBehaviour {

    public GamePhase gamePhase;
    public UIInputHandler uiInputHandler;
    public Map map;

    GameLogicOrchestrator gameLogicOrchestrator;

    void Awake() {
        gameLogicOrchestrator = new GameLogicOrchestrator(
            gamePhase: gamePhase,
            playerActionInput: uiInputHandler,
            pathingAndLOS: new SoldierPathingAndLOS(map)
        );
    }
}
