using UnityEngine;
using TMPro;

public class InfoPanel : MonoBehaviour {
    public static InfoPanel instance;
    public TMP_Text textField;

    void Awake() => instance = this;

    public void SetText(string text) => textField.text = text;
    public void ClearText() => textField.text = "";
}