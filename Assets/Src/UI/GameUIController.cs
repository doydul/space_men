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
    public Image blackFade;
    public TextMeshProUGUI currentPhaseText;
    public GameObject victoryPopup;
    
    UIAnimator fadeAnimator;

    void Awake() {
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

    public void ShowVictoryPopup() {
        victoryPopup.SetActive(true);
   }

    public void FadeToBlack(Action finished) {
        blackFade.enabled = true;
        fadeAnimator.Enqueue(1f, finished);
    }
}
