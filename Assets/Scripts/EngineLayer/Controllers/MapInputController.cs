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
    Tile[] tilesToSelect;
    public Tile selectedTile { get; private set; }

    void Awake() {
        instance = this;
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
        
        // Mouse Zoom
        float scaleFactor = 1 + Input.mouseScrollDelta.y / 40;
        var cameraDiff = cam.transform.position - map.transform.position;
        var translation = cameraDiff * scaleFactor - cameraDiff;
        translation.z = 0;
        map.transform.position -= translation;
        map.transform.localScale *= scaleFactor;
        
        // Cheats
        if (Input.GetKeyDown(KeyCode.S)) { // Skip Level
            Campaign.NextLevel(PlayerSave.current);
            UnityEngine.SceneManagement.SceneManager.LoadScene("CampaignUI");
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
