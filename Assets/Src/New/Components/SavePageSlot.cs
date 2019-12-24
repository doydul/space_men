using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ButtonHandler))]
public class SavePageSlot : MonoBehaviour {
    
    public Text text;

    public void SetText(string value) {
        text.text = value;
    }

    public void SetButtonAction(Action callback) {
        GetComponent<ButtonHandler>().action.AddListener(() => callback());
    }
}