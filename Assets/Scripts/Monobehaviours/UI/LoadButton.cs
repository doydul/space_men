using UnityEngine;
using TMPro;

public class LoadButton : MonoBehaviour {
    
    public int slotID;
    public int fontSizeWhenInUse = 18;
    
    public TMP_Text textElement;
    
    void Start() {
        var save = PlayerSave.Load(slotID);
        if (save != null) {
            textElement.fontSize = fontSizeWhenInUse;
            textElement.text = $"TIME 00:00:00         CREDITS {save.credits}";
        }
    }
}