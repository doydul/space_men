using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;

public class MissionCompleteViewController : SceneMenu {

    public const string sceneName = "MissionCompleteView";

    public List<MissionCompleteRewardPanelController> rewardPanels;
    public RewardClaimer rewardClaimer;

    public static void OpenMenu() {
        SceneManager.LoadScene(sceneName);
    }

    protected override void _Awake() {

    }

    public void Continue() {
        FadeToBlack(() => {
            if (Squad.currentMission != null) {
            } else {
                MainMenuController.OpenMenu();
            }
        });
    }
}
