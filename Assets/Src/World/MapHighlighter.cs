using System.Collections.Generic;
using UnityEngine;

public class MapHighlighter : MonoBehaviour {

    public Map map;
    public GamePhase gamePhase;

    List<Tile> highlightedTiles;

    ViewableState viewableState { get { return ViewableState.instance; } }
    Soldier selectedUnit;

    void Awake() {
        highlightedTiles = new List<Tile>();
    }

    void Update() {
        if (viewableState.selectedSoldier != selectedUnit) UpdateHighlights();
    }

    void UpdateHighlights() {
        ClearHighlights();
        if (selectedUnit != null) {
            HighlightTile(selectedUnit.tile, Color.white);
            if (viewableState.isMovementPhaseActive) {
                MovementPhaseHighlights();
            } else {
                ShootingPhaseHighlights();
            }
        }
    }

    void ClearHighlights() {
        foreach (var tile in highlightedTiles) {
            tile.ClearHighlight();
        }
        highlightedTiles.Clear();
    }

    void HighlightTile(Tile tile, Color color) {
        tile.Highlight(color);
        highlightedTiles.Add(tile);
    }

    void MovementPhaseHighlights() {
        var pathFinder = new PathFinder(new SoldierPathingWrapper(map), selectedUnit.gridLocation);
        foreach (Tile tile in map.EnumerateTiles()) {
            if (tile.open && !tile.occupied) {
                var targets = new List<Vector2>() { tile.gridLocation };
                var path = pathFinder.FindPath(targets);
                if (path != null) {
                    if (selectedUnit.tilesMoved + path.Count <= selectedUnit.baseMovement) {
                        HighlightTile(tile, Color.green);
                    } else if (path.Count <= selectedUnit.remainingMovement) {
                        HighlightTile(tile, Color.yellow);
                    }
                }
            }
        }
    }

    void ShootingPhaseHighlights() {
        foreach (var alien in map.GetActors<Alien>()) {
            var los = new LineOfSight(selectedUnit.gridLocation, alien.gridLocation, new SoldierLOSWrapper(map));
            if (!alien.dead && selectedUnit.WithinSightArc(alien.gridLocation) && !los.Blocked()) {
                HighlightTile(alien.tile, Color.red);
            }
        }
    }
 }
