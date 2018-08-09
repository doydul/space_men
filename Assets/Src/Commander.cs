using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Commander : MonoBehaviour {

    public Map map;
    public GameUIController UIController;
    public GamePhase gamePhase;
    public UnityEvent PlayerMoved;

    private MapHighlighter highlighter;
    private Tile selectedTile;
    private Soldier selectedUnit;

    void Awake() {
        highlighter = new MapHighlighter();
        if (PlayerMoved == null) PlayerMoved = new UnityEvent();
    }

    void Start() {
        gamePhase.MovementPhaseStart.AddListener(StartMovementPhase);
        gamePhase.ShootingPhaseStart.AddListener(StartShootingPhase);
    }

    public void ClickTile(Tile tile) {
        selectedTile = tile;
        var soldier = tile.GetActor<Soldier>();
        if (soldier != null) {
            SelectUnit(soldier);
        } else {
            if (gamePhase.movement) {
                Move(tile);
            } else {
                Shoot(tile);
            }
        }
        if (gamePhase.movement) {
            UIController.EnableTurnButtons();
        } else {
            UIController.DisableTurnButtons();
        }
    }

    public void PressTurnSoldier(Soldier.Direction direction) {
        if (selectedUnit != null) selectedUnit.TurnTo(direction);
    }

    private void SelectUnit(Soldier soldier) {
        if (selectedUnit != null) selectedUnit.Deselect();
        selectedUnit = soldier;
        selectedUnit.Select();
        highlighter.ClearHighlights();
        highlighter.HighlightTile(soldier.tile);
    }

    private void DeselectUnit() {
        selectedUnit.Deselect();
        selectedUnit = null;
        highlighter.ClearHighlights();
    }

    private void Move(Tile tile) {
        if (selectedUnit == null) return;
        var start = selectedUnit.gridLocation;
        var targets = new List<Vector2>() { tile.gridLocation };
        var path = new Path(new PathFinder(new SoldierPathingWrapper(map), start, targets).FindPath());
        if (tile.open && !tile.occupied && path.Count <= selectedUnit.remainingMovement) {
            selectedUnit.MoveTo(tile);
            selectedUnit.TurnTo(tile.gridLocation - path.Last());
            highlighter.ClearHighlights();
            highlighter.HighlightTile(tile);
            PlayerMoved.Invoke();
        } else {
            DeselectUnit();
        }
    }

    private void Shoot(Tile tile) {
        if (selectedUnit == null) return;
        var alien = tile.GetActor<Alien>();
        var los = new LineOfSight(selectedUnit.gridLocation, tile.gridLocation, new SoldierLOSWrapper(map));
        if (alien != null && selectedUnit.WithinSightArc(tile.gridLocation) && !los.Blocked()) {
            if (selectedUnit.hasAmmo) {
                SoldierAttack.Execute(selectedUnit, alien, map);
            }
        } else {
            highlighter.ClearHighlights();
            DeselectUnit();
        }
    }

    private void StartMovementPhase() {
        foreach (var soldier in map.GetActors<Soldier>()) {
            soldier.StartMovementPhase();
        }
    }

    private void StartShootingPhase() {
        UIController.DisableTurnButtons();
        foreach (var soldier in map.GetActors<Soldier>()) {
            soldier.StartShootingPhase();
        }
    }
}
