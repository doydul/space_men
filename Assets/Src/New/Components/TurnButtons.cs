using UnityEngine;

public class TurnButtons : MonoBehaviour {

    public static TurnButtons instance { get; private set; }

    void Awake() {
        instance = this;
        Hide();
    }
    
    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }
}