using UnityEngine;
using TMPro;

public class SideModal : MonoBehaviour {

    public TMP_Text modalText;
    
    public static SideModal instance;
    
    public bool isOpen => gameObject.activeSelf;
    
    void Awake() => instance = this;
    void Start() => Hide();
    
    public void Show(string text) {
        modalText.text = text;
        gameObject.SetActive(true);
    }
    
    public void Hide() => gameObject.SetActive(false);
}