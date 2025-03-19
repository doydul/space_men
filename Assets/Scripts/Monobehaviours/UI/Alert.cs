using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class Alert : MonoBehaviour {
    
    TMP_Text textEl;
    
    static Alert instance;
    void Start() {
        instance = this;
        textEl = GetComponent<TMP_Text>();
        GameEvents.On(this, "alien_turn_end", Clear);
        Clear();
    }
    
    void OnDestroy() {
        GameEvents.RemoveListener(this, "alien_turn_end");
    }
    
    void Clear() {
        textEl.text = "";
    }
    
    public static void Show(string text) {
        instance.textEl.text = text;
    }
}