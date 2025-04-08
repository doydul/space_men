using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class MapHighlighter : MonoBehaviour {

    public static MapHighlighter instance { get; private set; }

    public Map map;

    List<Tile> highlightedTiles = new();
    List<Border> borders = new();

    Soldier selectedUnit;
    Vector2 selectedUnitGridLocation;
    bool movementPhaseActive;

    int updateCounter;

    void Awake() {
        instance = this;
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
        foreach (var border in borders) border.Remove();
        highlightedTiles.Clear();
        borders.Clear();
    }

    public void HighlightTile(Tile tile, Color color) {
        tile.Highlight(color);
        highlightedTiles.Add(tile);
    }
    
    public void BorderTiles(IEnumerable<Tile> tiles, Color color) {
        var border = SFXLayer.instance.SpawnBorder(tiles.Select(tile => (Vector2)tile.realLocation).ToList());
        border.SetColor(color);
        borders.Add(border);
    }
 }
