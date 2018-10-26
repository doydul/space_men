using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;

public class GameUIController : MonoBehaviour {

    public Commander commander;
    public GameObject turnButtonContainer;
    public Image blackFade;
    public Text currentPhaseText;
    public GameObject victoryPopup;

    private UIAnimator fadeAnimator;

    void Awake() {
        DisableTurnButtons();
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

    public void PressTurnSoldier(int direction) {
        commander.PressTurnSoldier((Soldier.Direction)direction);
    }

    public void EnableTurnButtons() {
        turnButtonContainer.SetActive(true);
    }

    public void DisableTurnButtons() {
        turnButtonContainer.SetActive(false);
    }

    public void SetMovementPhaseText() {
        currentPhaseText.text = "Movement Phase";
    }

    public void SetShootingPhaseText() {
        currentPhaseText.text = "Shooting Phase";
    }

    public void ShowVictoryPopup() {
        victoryPopup.SetActive(true);
    }

    public void FadeToBlack(Action finished) {
        blackFade.enabled = true;
        fadeAnimator.Enqueue(1f, finished);
    }
}
