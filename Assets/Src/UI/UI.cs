using UnityEngine;
using System;

public class UI : MonoBehaviour, IUserInterface {

    public static UI instance;

    public GamePhase gamePhase;
    public GameUIController gameUIController;

    void Awake() {
        instance = this;
    }

    public void Select(Soldier soldier) {
        soldier.GetComponent<SoldierUIController>().Select();
    }

    public void Deselect(Soldier soldier) {
        soldier.GetComponent<SoldierUIController>().Deselect();
    }

    public void ShowMovementIndicators(Soldier soldier) {
        soldier.GetComponent<SoldierUIController>().ShowMovementIndicators();
    }

    public void ShowAmmoIndicators(Soldier soldier) {
        soldier.GetComponent<SoldierUIController>().ShowAmmoIndicators();
    }

    public void ShowVictoryPopup() {
        gameUIController.ShowVictoryPopup();
    }

    public void FadeToBlack(Action finished) {
        gameUIController.FadeToBlack(finished);
    }
}
