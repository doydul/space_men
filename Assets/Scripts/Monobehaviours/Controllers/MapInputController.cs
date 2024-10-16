using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MapInputController : MonoBehaviour {

    public static MapInputController instance;

    public Camera cam;
    public Map map;

    bool dragging;
    bool dragged;
    bool uiClicked;
    Vector3 dragStartPosition;
    Vector3 mapStartPosition;
    float touchSeperation;
    Tile[] tilesToSelect;
    public Tile selectedTile { get; private set; }
    
    void Awake() {
        instance = this;
    }
    
    bool IsPointerOverGameObject() {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return true;
        
        if(Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began){
            if(UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId)) return true;
        }
        
        return false;
    }

    void Update() {
        // Mouse Zoom
        float scaleFactor = 1 + Input.mouseScrollDelta.y / 40;
        
        // Pinch Zoom
        if (Input.touchCount == 2) {
            dragging = false;
            var currentDist = (Input.GetTouch(0).position - Input.GetTouch(1).position).magnitude;
            if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(1).phase == TouchPhase.Began) touchSeperation = currentDist;
            scaleFactor = touchSeperation / currentDist;
            touchSeperation = currentDist;
        } else {
            HandleDrag();
        }
        if (map.transform.localScale.x * scaleFactor > 2) scaleFactor = 2 / map.transform.localScale.x;
        if (map.transform.localScale.x * scaleFactor < 0.5f) scaleFactor = 0.5f / map.transform.localScale.x;
        var cameraDiff = cam.transform.position - map.transform.position;
        var translation = cameraDiff * scaleFactor - cameraDiff;
        translation.z = 0;
        map.transform.position -= translation;
        map.transform.localScale *= scaleFactor;
    }
    
    void HandleDrag() {
        if (dragging && (Input.mousePosition - dragStartPosition).magnitude > 10) {
            dragged = true;
            Vector3 delta = Input.mousePosition - dragStartPosition;
            var newPos = mapStartPosition + (delta * 0.03f);
            var diff = cam.transform.position - newPos;
            var mapWidth = map.tiles.GetLength(0) * map.transform.localScale.x;
            var mapHeight = map.tiles.GetLength(1) * map.transform.localScale.x;
            if (diff.x < 0) newPos.x += diff.x;
            if (diff.x > mapWidth) newPos.x += diff.x - mapWidth;
            if (diff.y < 0) newPos.y += diff.y;
            if (diff.y > mapHeight) newPos.y += diff.y - mapHeight;
            map.transform.position = newPos;
        }
        if (Input.GetMouseButtonDown(0)) {
            if (IsPointerOverGameObject()) {
                uiClicked = true;
            } else {
                dragStartPosition = Input.mousePosition;
                mapStartPosition = map.transform.position;
                dragging = true;
            }
        }
        if (Input.GetMouseButtonUp(0)) {
            if (!IsPointerOverGameObject() && !uiClicked && !dragged) {
                Click(Input.mousePosition);
            }
            dragging = false;
            dragged = false;
            uiClicked = false;
        }
    }

    public void Click(Vector2 mousePosition) {
        RaycastHit hit;
        Physics.Raycast(cam.ScreenPointToRay(mousePosition), out hit);
        if (hit.collider != null) {
            var tile = hit.collider.gameObject.GetComponent<Tile>();
            if (tile != null) {
                if (tilesToSelect != null && tilesToSelect.Contains(tile)) {
                    selectedTile = tile;
                    tilesToSelect = null;
                } else if (!AnimationManager.instance.animationInProgress) {
                    if (UIState.instance.IsActorSelected()) {
                        UIState.instance.GetSelectedActor().Interact(tile);
                    } else {
                        var actor = tile.GetActor<Actor>();
                        if (actor != null) {
                            actor.Select();
                        }
                    }
                }
            }
        }
    }

    public IEnumerator SelectTileFrom(Color highlightColor, Tile[] tiles) {
        tilesToSelect = tiles;
        selectedTile = null;
        MapHighlighter.instance.ClearHighlights();
        foreach (var tile in tiles) {
            MapHighlighter.instance.HighlightTile(tile, highlightColor);
        }
        while (tilesToSelect != null && selectedTile == null) {
            yield return null;
        }
        MapHighlighter.instance.ClearHighlights();
    }

    public void CancelTileSelect() {
        MapHighlighter.instance.ClearHighlights();
        tilesToSelect = null;
    }
}
