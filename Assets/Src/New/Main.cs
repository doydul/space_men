using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

    public GamePhase gamePhase;
    public UIInputHandler uiInputHandler;
    public Map map;
    public WorldComponent worldComponent;
    public Camera mainCamera;

    GameLogicOrchestrator gameLogicOrchestrator;
    public static Main instance;

    void Start() {
        instance = this;
        gameLogicOrchestrator = new GameLogicOrchestrator(
            gamePhase: gamePhase,
            playerActionInput: uiInputHandler,
            pathingAndLOS: new SoldierPathingAndLOS(map),
            gameMap: new GameMapImpl(map),
            cameraController: new CameraController(mainCamera),
            world: worldComponent,
            legacyMap: map
        );
    }
}
