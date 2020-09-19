using UnityEngine;

public class WorldButtonsContainer : MonoBehaviour {

    public static WorldButtonsContainer instance { get; set; }
    
    WorldButton[] buttons;

    void Awake() {
        instance = this;
        buttons = transform.GetComponentsInChildren<WorldButton>();
    }

    public void HideAll() {
        foreach (var button in buttons) {
            button.Hide();
        }
    }
}