using System.Collections.Generic;
using UnityEngine;

using Data;

public class MapHighlighter : MonoBehaviour {

    public static MapHighlighter instance { get; private set; }

    public Map map;

    List<Tile> highlightedTiles;

    ViewableState viewableState { get { return ViewableState.instance; } }
    WorldState worldState { get { return WorldState.instance; } }
    Soldier selectedUnit;
    Vector2 selectedUnitGridLocation;
    bool movementPhaseActive;

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
 }
