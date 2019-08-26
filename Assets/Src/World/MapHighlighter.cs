using System.Collections.Generic;
using UnityEngine;

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

    void Update() {
        if (animating != worldState.animationInProgress) {
            animating = worldState.animationInProgress;
            if (animating) {
                ClearHighlights();
            } else {
                UpdateHighlights();
            }
        }
        if (!animating) {
            if (movementPhaseActive != viewableState.isMovementPhaseActive) {
                UpdateHighlights();
                movementPhaseActive = viewableState.isMovementPhaseActive;
            }
            if (selectedUnit != viewableState.selectedSoldier) {
                selectedUnit = viewableState.selectedSoldier;
                if (selectedUnit != null) selectedUnitGridLocation = selectedUnit.gridLocation;
                UpdateHighlights();
            } else if (updateCounter >= 5) {
                UpdateShootingPhaseHighlights();
                updateCounter = 0;
            }
            updateCounter++;
        }
    }

    void UpdateHighlights() {
        ClearHighlights();
        if (selectedUnit != null) {
            HighlightTile(selectedUnit.tile, Color.white);
            if (viewableState.isMovementPhaseActive) {
                // MovementPhaseHighlights();
                mapController.ShowPossibleMovesFor(selectedUnit.index);
            } else {
                ShootingPhaseHighlights();
            }
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

    void UpdateShootingPhaseHighlights() {
        if (!movementPhaseActive) {
            UpdateHighlights();
        }
    }
 }
