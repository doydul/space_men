using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollableSelector : MonoBehaviour {
    
    public ScrollRect scrollRect;
    public Transform content;

    List<ButtonHandler> buttonHandlers;
    bool buttonsDisabled;

    void Awake() {
        buttonHandlers = new List<ButtonHandler>();
        scrollRect.normalizedPosition = Vector2.zero;
    }

    void Update() {
        if (!buttonsDisabled && Mathf.Abs(scrollRect.velocity.x) > 1) {
            DisableButtons();
        } else if (buttonsDisabled && Mathf.Abs(scrollRect.velocity.x) <= 1) {
            EnableButtons();
        }
    }

    public void AddButton(Transform button) {
        button.SetParent(content, false);
        var buttonHandler = button.GetComponent<ButtonHandler>();
        if (buttonHandler != null) {
            buttonHandlers.Add(buttonHandler);
        } else {
            Debug.LogError("Object must have a ButtonHandler component!", button);
        }
    }

    void DisableButtons() {
        buttonsDisabled = true;
        foreach (var button in buttonHandlers) {
            button.Disable();
        }
    }

    void EnableButtons() {
        buttonsDisabled = false;
        foreach (var button in buttonHandlers) {
            button.Enable();
        }
    }
}