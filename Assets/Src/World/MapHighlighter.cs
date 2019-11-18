using System.Collections.Generic;
using UnityEngine;

using Data;

public class MapHighlighter : MonoBehaviour {

    public static MapHighlighter instance { get; private set; }

    public Map map;
    public GamePhase gamePhase;
    public MapController mapController;

    List<Tile> highlightedTiles;

    ViewableState viewableState { get { return ViewableState.instance; } }
    WorldState worldState { get { return WorldState.instance; } }
    Soldier selectedUnit;
    Vector2 selectedUnitGridLocation;
    bool movementPhaseActive;
    bool animating;

    int updateCounter;

    void Awake() {
        instance = this;
        highlightedTiles = new List<Tile>();
        movementPhaseActive = true;
    }

    void UpdateHighlights() {
        ClearHighlights();
        if (selectedUnit != null) {
            HighlightTile(selectedUnit.tile, Color.white);
        }
    }

    public void ClearHighlights() {
        foreach (var tile in highlightedTiles) {
            tile.ClearHighlight();
        }
        highlightedTiles.Clear();
    }

    public void HighlightTile(Tile tile, Color color) {
        tile.Highlight(color);
        highlightedTiles.Add(tile);
    }

    public void HighlightPossibleActions(ActorAction[] actorActions) {
        ClearHighlights();
        if (actorActions == null) return;
        foreach (var action in actorActions) {
            if (action.type == ActorActionType.Move) {
                var color = action.sprint ? Color.yellow : Color.green;
                HighlightTile(map.GetTileAt(new Vector2(action.target.x, action.target.y)), color);
            } else if (action.type == ActorActionType.Shoot) {
                HighlightTile(map.GetTileAt(new Vector2(action.target.x, action.target.y)), Color.red);
            } else if (action.type == ActorActionType.PossibleMove) {
                HighlightTile(map.GetTileAt(new Vector2(action.target.x, action.target.y)), Color.red);
            }
        }
    }
 }
