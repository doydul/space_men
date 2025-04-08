using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel.Design;

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
    
    public void BorderTile(Tile tile, Color color) => BorderTiles( new[] { tile}, color);
    public void BorderTiles(IEnumerable<Tile> tiles, Color color) {
        if (!tiles.Any()) return;
        var continuousGroups = new List<List<Tile>>();
        var tilesList = tiles.ToList();
        continuousGroups.Add(new List<Tile>());
        var firstTile = tilesList[0];
        tilesList.Remove(firstTile);
        var activeGroup = continuousGroups[0];
        activeGroup.Add(firstTile);
        while (tilesList.Count > 0) {
            bool tileAdded = false;
            foreach (var tile in new List<Tile>(tilesList)) {
                if (activeGroup.Any(activeTile => Map.instance.ManhattanDistance(activeTile.gridLocation, tile.gridLocation) == 1)) {
                    tilesList.Remove(tile);
                    activeGroup.Add(tile);
                    tileAdded = true;
                }
            }
            if (!tileAdded) {
                var nextTile = tilesList[0];
                tilesList.Remove(nextTile);
                activeGroup = new() { nextTile };
                continuousGroups.Add(activeGroup);
            }
        }
        
        foreach (var group in continuousGroups) {
            var border = SFXLayer.instance.SpawnBorder(group.Select(tile => tile.realLocation).ToList());
            border.SetColor(color);
            borders.Add(border);
        }
    }
 }
