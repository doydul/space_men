using UnityEngine;
using System.Collections;
using TMPro;

public class ConfirmationPopup : MonoBehaviour {
    
    public static ConfirmationPopup instance;
    
    public TMP_Text tmpText;
    
    public bool confirmed { get; private set; }
    bool answered;
    
    void Awake() => instance = this;
    
    void Start() => gameObject.SetActive(false);
    
    public IEnumerator AskForConfirmation(string text) {
        gameObject.SetActive(true);
        tmpText.text = text;
        answered = false;
        confirmed = false;
        while (!answered) {
            yield return null;
        }
    }
    
    public void Cancel() {
        answered = true;
        gameObject.SetActive(false);
    }
    
    public void Ok() {
        confirmed = true;
        answered = true;
        gameObject.SetActive(false);
    }
}