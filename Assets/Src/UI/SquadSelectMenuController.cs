using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SquadSelectMenuController : MonoBehaviour {
    
    public Image blackFade;
    public List<SquadPanelController> squadPanels;
    public string nextScene;
    public string previousScene;
    
    private UIAnimator fadeAnimator;
    
    void Awake() {
        DataPersistence.Load();
        
        for (int i = 0; i < DataPersistence.squads.Count; i++) {
            squadPanels[i].squad = DataPersistence.squads[i];
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
    
    public void Next(Squad squad) {
        if (squad != null) {
            Squad.SetActive(squad);
        } else {
            Squad.SetActive(Squad.GenerateDefault());
        }
        
        blackFade.enabled = true;
        fadeAnimator.Enqueue(1f, () => {
            SceneManager.LoadScene(nextScene);
        });
    }
    
    public void Back() {
        blackFade.enabled = true;
        fadeAnimator.Enqueue(1f, () => {
            SceneManager.LoadScene(previousScene);
        });
    }
}