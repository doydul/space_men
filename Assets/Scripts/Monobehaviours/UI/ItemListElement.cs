using UnityEngine;
using UnityEngine.UI;

public class ItemListElement : MonoBehaviour {
    
    public Image highlight;
    public Image blueprintBackground;
    
    bool _isBlueprint;
    public bool isBlueprint {
        get => _isBlueprint;
        set {
            _isBlueprint = value;
            blueprintBackground.enabled = isBlueprint;
        }
    }
    
    void Start() {
        Deselect();
    }
    
    public void Select() {
        highlight.enabled = true;
    }
    
    public void Deselect() {
        highlight.enabled = false;
    }
}