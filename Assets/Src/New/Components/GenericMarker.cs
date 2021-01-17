using UnityEngine;
using TMPro;

public class GenericMarker : MonoBehaviour {
    
    public TMP_Text text;

    public void SetText(string text) {
        this.text.text = text;
    }
}