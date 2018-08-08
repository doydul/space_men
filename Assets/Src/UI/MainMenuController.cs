using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {
    
    public Image blackFade;
    public string startScene;
    
    private UIAnimator fadeAnimator;
    
    void Start() {
        fadeAnimator = new UIAnimator(0f, 1f, this, (value) => {
            var temp = blackFade.color;
            temp.a = value;
            blackFade.color = temp;
        });
    }
    
    public void NewGame() {
        blackFade.enabled = true;
        fadeAnimator.Enqueue(1f, () => {
            SceneManager.LoadScene(startScene);
        });
    }
}