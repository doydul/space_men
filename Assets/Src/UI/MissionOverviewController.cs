using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MissionOverviewController : SceneMenu {

    public const string sceneName = "MissionOverview";

    public Text briefingText;

    public static void OpenMenu() {
        SceneManager.LoadScene(sceneName);
    }

    protected override void _Awake() {
        briefingText.text = Squad.currentMission.briefing;
    }

    public void GoToSquadEditScreen() {
        ArmouryMenuController.OpenMenu();
    }

    public void Continue() {
        FadeToBlack(() => {
            GameplayOrchestrator.StartGame();
        });
    }
}
