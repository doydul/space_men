using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;

using TMPro;

public class GameUIController : MonoBehaviour {

    public GameObject turnButtonContainer;
    public TextMeshProUGUI currentPhaseText;
    public GameObject victoryPopup;
    
    UIAnimator fadeAnimator;

    void Awake() {
        victoryPopup.SetActive(false);
    }

    public void ShowVictoryPopup() {
        victoryPopup.SetActive(true);
   }

    public void FadeToBlack(Action finished) {
    }
}
