using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ArmouryMenuController : SceneMenu {

    public const string sceneName = "Armoury";

    public string templarScene;
    public string missionScene;
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
            SceneManager.LoadScene(missionScene);
        });
    }

    void ViewTemplar() {
        FadeToBlack(() => {
            SceneManager.LoadScene(templarScene);
        });
    }

    public void ClickSoldier(int soldierIndex) {
        FadeToBlack(() => {
            SoldierSelectMenuController.OpenMenu(soldierIndex);
        });
    }

    public void ViewInvectory(SoldierData soldier) {
        TemplarViewController.activeSoldier = soldier;
        ViewTemplar();
    }
}
