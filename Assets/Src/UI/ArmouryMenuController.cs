using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ArmouryMenuController : SceneMenu {

    public const string sceneName = "Armoury";

    public SoldierPanelController[] soldierPanelControllers;

    public static void OpenMenu() {
        SceneManager.LoadScene(sceneName);
    }

    protected override void _Awake() {
        if (Squad.active == null) Squad.SetActive(Squad.GenerateDefault());

        for (int i = 0; i < soldierPanelControllers.Length; i++) {
            soldierPanelControllers[i].soldierIndex = i;
        }
    }

    public void Continue() {
        FadeToBlack(() => {
            MissionOverviewController.OpenMenu();
        });
    }

    public void ViewSoldier(SoldierData soldier) {
        FadeToBlack(() => {
            SoldierViewController.OpenMenu(soldier);
        });
    }

    public void ClickSoldier(int soldierIndex) {
        FadeToBlack(() => {
            SelectionMenuController.OpenMenu(soldierIndex);
        });
    }
}
