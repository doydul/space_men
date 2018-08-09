using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameUIController : MonoBehaviour {
    
    public Commander commander;
    public GameObject turnButtonContainer;
    public Image blackFade;
    
    private UIAnimator fadeAnimator;
    
    void Awake() {
        DisableTurnButtons();
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
    
    public void PressTurnSoldier(int direction) {
        commander.PressTurnSoldier((Soldier.Direction)direction);
    }
    
    public void EnableTurnButtons() {
        turnButtonContainer.SetActive(true);
    }
    
    public void DisableTurnButtons() {
        turnButtonContainer.SetActive(false);
    }
}