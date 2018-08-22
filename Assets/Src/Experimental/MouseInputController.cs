using UnityEngine;

public class MouseInputController : MonoBehaviour {

    public InputManager inputManager;

    private bool dragging;
    private bool dragged;
    private Vector3 dragStartPosition;

    void Update() {
        if (dragging && (Input.mousePosition - dragStartPosition).magnitude > 10) {
            dragged = true;
            Vector3 delta = Input.mousePosition - dragStartPosition;
            inputManager.DragMouse((Vector2)delta);
            dragStartPosition = Input.mousePosition;
        }
        if (Input.GetMouseButtonDown(0)) {
            dragStartPosition = Input.mousePosition;
            dragging = true;
        }
        if (Input.GetMouseButtonUp(0)) {
            if (!dragged) inputManager.Click(Input.mousePosition);
            dragging = false;
            dragged = false;
        }
    }
}
