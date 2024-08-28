using UnityEngine;
using TMPro;

public class TutorialPopup : MonoBehaviour {
    
    public TMP_Text tutorialText;
    
    public void SetText(string text) {
        tutorialText.text = text;
    }
    
    public void Ok() {
        Destroy(gameObject);
    }
}