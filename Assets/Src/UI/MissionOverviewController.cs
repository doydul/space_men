using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MissionOverviewController : MonoBehaviour {
    
    public Image blackFade;
    public Text briefingText;
    
    private UIAnimator fadeAnimator;
    
    void Awake() {
        briefingText.text = Squad.currentMission.briefing;
        
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
            SceneManager.LoadScene(Squad.currentMission.sceneName);
        });
    }
}