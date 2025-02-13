using UnityEngine;
using UnityEngine.UI;

public class SquadSoldierIcon : MonoBehaviour {
    
    public Image icon;
    public Image highlight;
    
    public bool selected { get; private set; }
    
    void Start() => Deselect();
    
    public void ShowSoldier() => icon.enabled = true;
    public void HideSoldier() => icon.enabled = false;
    
    public void Select() {
        selected = true;
        highlight.enabled = true;
    }
    
    public void Deselect() {
        selected = false;
        highlight.enabled = false;
    }
}