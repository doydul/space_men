using UnityEngine;
using TMPro;

public class InformationPanel : MonoBehaviour {
    public static InformationPanel instance;
    public TMP_Text textField;

    void Awake() => instance = this;

    public void SetText(string text) => textField.text = text;
    public void ClearText() => textField.text = "";
}