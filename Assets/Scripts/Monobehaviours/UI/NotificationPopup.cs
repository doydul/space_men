using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class NotificationPopup : MonoBehaviour {
    
    static NotificationPopup instance;
    
    void Awake() => instance = this;
    
    void Start() {
        buttonPrototype.gameObject.SetActive(false);
        Close();
    }
    
    public TMP_Text titleText;
    public TMP_Text contentText;
    public Transform buttonPrototype;
    
    public static IEnumerator PerformShow(string title, string content, params BtnData[] btns) {
        instance.gameObject.SetActive(true);
        instance.titleText.text = title;
        instance.contentText.text = content;
        
        instance.buttonPrototype.parent.DestroyChildren(1);
        bool pressed = false;
        foreach (var btn in btns) {
            var btnTrans = NotificationPopup.Instantiate(instance.buttonPrototype, instance.buttonPrototype.parent);
            btnTrans.gameObject.SetActive(true);
            var textComponent = btnTrans.GetComponentInChildren<TMP_Text>();
            textComponent.text = btn.label;
            var buttonComponent = btnTrans.GetComponentInChildren<ButtonHandler>();
            buttonComponent.action.AddListener(() => {
                pressed = true;
                Close();
                btn.callback();
            });
        }
        while (!pressed) {
            yield return null;
        }
    }
    
    public static void Close() {
        instance.gameObject.SetActive(false);
    }
}