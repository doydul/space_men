using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialPopup : MonoBehaviour {
    
    public enum Offset {
        Above,
        Below,
        Left,
        Right
    }
    
    public TMP_Text tutorialText;
    public Transform panel;
    public GameObject hole;
    public GameObject backdrop;
    public float holeOffset;
    
    bool hideHole = true;
    
    void Start() {
        if (hideHole) HideHole();
        else backdrop.SetActive(false);
    }
    
    public void SetText(string text) {
        tutorialText.text = text;
    }
    
    public void ApplyOffset(Offset offset) {
        hideHole = false;
        if (offset == Offset.Above) {
            panel.localPosition += new Vector3(0, holeOffset, 0);
        } else if (offset == Offset.Below) {
            panel.localPosition += new Vector3(0, -holeOffset, 0);
        } else if (offset == Offset.Left) {
            panel.localPosition += new Vector3(-holeOffset, 0, 0);
        } else {
            panel.localPosition += new Vector3(holeOffset, 0, 0);
        }
    }
    
    public void HideHole() => hole.SetActive(false);
    
    public void Ok() {
        Destroy(gameObject);
    }
}