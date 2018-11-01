using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ArmouryMenuController : MonoBehaviour {

    public Image blackFade;
    public string templarScene;
    public string missionScene;
    public SoldierPanelController[] soldierPanelControllers;

    private UIAnimator fadeAnimator;

    void Awake() {
        if (Squad.active == null) Squad.SetActive(Squad.GenerateDefault());

        for (int i = 0; i < soldierPanelControllers.Length; i++) {
            soldierPanelControllers[i].soldier = Squad.GetSoldier(i);
        }

        fadeAnimator = new UIAnimator(1f, 1f, this, (value) => {
            var temp = blackFade.color;
            temp.a = value;
            blackFade.color = temp;
        });

        blackFade.enabled = true;
    }

    void Start() {
        fadeAnimator.Enqueue(0f, () => {
            blackFade.enabled = false;
        });
    }

    public void Continue() {
        blackFade.enabled = true;
        fadeAnimator.Enqueue(1f, () => {
            SceneManager.LoadScene(missionScene);
        });
    }

    void ViewTemplar() {
        blackFade.enabled = true;
        fadeAnimator.Enqueue(1f, () => {
            SceneManager.LoadScene(templarScene);
        });
    }

    public void ClickSoldier(SoldierData soldier) {
        // Goto soldier select screen
    }

    public void ViewInvectory(SoldierData soldier) {
        TemplarViewController.activeSoldier = soldier;
        ViewTemplar();
    }
}
