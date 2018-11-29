using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameplayOrchestrator : MonoBehaviour {

    public Map map;

    public GameLogicComponent gameLogic;
    public GameEvents events;
    public WorldComponent world;
    public UI ui;

    public static void StartGame() {
        SceneManager.LoadScene(Squad.currentMission.sceneName);
    }

    void Awake() {
        gameLogic.SetWorld(world);
        gameLogic.SetUserInterface(ui);
    }

    public void EndMission() {
        ui.FadeToBlack(() => {
            MissionCompleteViewController.OpenMenu();
            Squad.IncrementMission();
        });
    }
}
