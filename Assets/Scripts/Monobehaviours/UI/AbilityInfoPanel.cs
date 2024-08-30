using UnityEngine;
using TMPro;

public class AbilityInfoPanel : MonoBehaviour {

    public static AbilityInfoPanel instance;
    void Awake() => instance = this;
    void Start() => Hide();

    public TMP_Text textElement;

    public void Show() {
        gameObject.SetActive(true);
        UIManager.HideNormalGameHud();
    }
    public void Hide() {
        gameObject.SetActive(false);
        UIManager.ShowNormalGameHud();
    }
    
    public void ShowDescription(string text) {
        textElement.text = text;
        Show();
    }

    public void Cancel() {
        MapInputController.instance.CancelTileSelect();
        Hide();
    }
}