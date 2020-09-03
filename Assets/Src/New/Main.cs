using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

    public GamePhase gamePhase;
    public UIInputHandler uiInputHandler;
    public Map map;
    public WorldComponent worldComponent;
    public Camera mainCamera;

    public AnimationInteractor animationInteractor;
    
    public UIController uiController;

    GameLogicOrchestrator gameLogicOrchestrator;
    public static Main instance;

    void Awake() {
        Application.targetFrameRate = 30;
        instance = this;

        var worldAnimator = new WorldAnimator(animationInteractor);

        gameLogicOrchestrator = new GameLogicOrchestrator(
            gamePhase: gamePhase,
            playerActionInput: uiInputHandler,
            pathingAndLOS: new SoldierPathingAndLOS(map),
            gameMap: new GameMapImpl(map),
            cameraController: new CameraController(mainCamera),
            animationReel: worldAnimator,
            world: worldComponent,
            legacyMap: map,
            uiController: uiController
        );
    }
}
