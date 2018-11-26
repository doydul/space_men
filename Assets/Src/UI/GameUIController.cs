using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;

public class GameUIController : MonoBehaviour {

    public GameObject turnButtonContainer;
    public Image blackFade;
    public Text currentPhaseText;
    public GameObject victoryPopup;

    UIAnimator fadeAnimator;
    UIHelper uiHelper;

    void Awake() {
        uiHelper = new UIHelper(turnButtonContainer, currentPhaseText);
        fadeAnimator = new UIAnimator(1f, 1f, this, (value) => {
            var temp = blackFade.color;
            temp.a = value;
            blackFade.color = temp;
        });

        blackFade.enabled = true;

        victoryPopup.SetActive(false);
    }

    void Start() {
        fadeAnimator.Enqueue(0f, () => {
            blackFade.enabled = false;
        });
    }

    void Update() {
        uiHelper.Update(ViewableState.instance);
    }

    public void ShowVictoryPopup() {
        victoryPopup.SetActive(true);
    }

    public void FadeToBlack(Action finished) {
        blackFade.enabled = true;
        fadeAnimator.Enqueue(1f, finished);
    }

    private class UIHelper {

        public UIHelper(GameObject turnButtonContainer, Text currentPhaseText) {
            this.turnButtonContainer = turnButtonContainer;
            this.currentPhaseText = currentPhaseText;
            turnButtonContainer.SetActive(false);
        }

        GameObject turnButtonContainer;
        Text currentPhaseText;

        public void Update(ViewableState viewableState) {
            turnButtonContainer.SetActive(viewableState.canTurnSoldier);
            if (viewableState.isMovementPhaseActive) {
                currentPhaseText.text = "Movement Phase";
            } else {
                currentPhaseText.text = "Shooting Phase";
            }
        }
    }
}
