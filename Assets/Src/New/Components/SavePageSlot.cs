using UnityEngine;
using UnityEngine.UI;

public class SavePageSlot : MonoBehaviour {
    
    public Text text;

    public void SetText(string value) {
        text.text = value;
    }
}