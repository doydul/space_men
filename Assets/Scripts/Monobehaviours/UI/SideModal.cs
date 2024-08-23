using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using TMPro;

public class SideModal : MonoBehaviour {

    public TMP_Text modalText;
    
    public static SideModal instance;
    void Awake() => instance = this;
    void Start() => Hide();

    public void Show(string text) {
        modalText.text = text;
        gameObject.SetActive(true);
    }
    
    public void Hide() => gameObject.SetActive(false);
}