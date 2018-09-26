using UnityEngine;
using System.Collections.Generic;

public class MapInputController : MonoBehaviour {

    public Camera cam;
    public Map map;
    public Commander commander;

    private bool dragging;
    private bool dragged;
    private Vector3 dragStartPosition;
    private Vector3 mapStartPosition;

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
            map.transform.position = mapStartPosition + (delta * 0.01f);
        }
        if (Input.GetMouseButtonDown(0)) {
            dragStartPosition = Input.mousePosition;
            mapStartPosition = map.transform.position;
            dragging = true;
        }
        if (Input.GetMouseButtonUp(0)) {
            if (!dragged) Click(Input.mousePosition);
            dragging = false;
            dragged = false;
        }
        if (Input.GetKeyDown("-")) {
          map.transform.localScale = map.transform.localScale * 0.9f;
        }
        if (Input.GetKeyDown("=")) {
          map.transform.localScale = map.transform.localScale * 1.1f;
        }
    }

    public void Click(Vector2 mousePosition) {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;
        RaycastHit hit;
        Physics.Raycast(cam.ScreenPointToRay(mousePosition), out hit);
        if (hit.collider != null) {
            var tile = hit.collider.gameObject.GetComponent<Tile>();
            if (tile != null) {
                commander.ClickTile(tile);
            }
        }
    }
}
