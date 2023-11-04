using UnityEngine;
using System.Collections.Generic;

public class MapInputController : MonoBehaviour {

    public Camera cam;
    public Map map;

    bool dragging;
    bool dragged;
    bool uiClicked;
    Vector3 dragStartPosition;
    Vector3 mapStartPosition;

    void Awake() {
        if (Squad.active == null) {
            DataPersistence.Load();
            Squad.SetActive(Squad.GenerateDefault());
        }
    }

    void Update() {
        if (dragging && (Input.mousePosition - dragStartPosition).magnitude > 10) {
            dragged = true;
            Vector3 delta = Input.mousePosition - dragStartPosition;
            map.transform.position = mapStartPosition + (delta * 0.03f);
        }
        if (Input.GetMouseButtonDown(0)) {
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) {
                uiClicked = true;
            } else {
                dragStartPosition = Input.mousePosition;
                mapStartPosition = map.transform.position;
                dragging = true;
            }
        }
        if (Input.GetMouseButtonUp(0)) {
            if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() && !uiClicked && !dragged) {
                Click(Input.mousePosition);
            }
            dragging = false;
            dragged = false;
            uiClicked = false;
        }
        if (Input.GetKeyDown("-")) {
          map.transform.localScale = map.transform.localScale * 0.9f;
        }
        if (Input.GetKeyDown("=")) {
          map.transform.localScale = map.transform.localScale * 1.1f;
        }
    }

    public void Click(Vector2 mousePosition) {
        RaycastHit hit;
        Physics.Raycast(cam.ScreenPointToRay(mousePosition), out hit);
        if (hit.collider != null) {
            var tile = hit.collider.gameObject.GetComponent<Tile>();
            if (tile != null) {
                // TODO interact with tile when clicked
            }
        }
    }
}
