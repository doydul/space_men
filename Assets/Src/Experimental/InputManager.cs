using UnityEngine;
using System;

public class InputManager : MonoBehaviour, IMouseDragManager, IGameButtonManager {

    public event Action<Vector2> MouseDragged;
    public event Action ProceedButtonPressed;

    public void DragMouse(Vector2 translation) {
        if (MouseDragged != null) MouseDragged(translation);
    }

    public void PressProceedButton() {
        if (ProceedButtonPressed != null) ProceedButtonPressed();
    }

    public void Click(Vector2 mousePosition) {

    }
}
