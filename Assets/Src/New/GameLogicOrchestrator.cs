using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

public class GameLogicOrchestrator {

    GamePhase gamePhase;
    PlayerActionHandler playerActionHandler;
    SoldierActionHandler soldierActionHandler;

    public GameLogicOrchestrator(GamePhase gamePhase, PlayerActionHandler.IPlayerActionInput playerActionInput, SoldierActionHandler.IPathingAndLOS pathingAndLOS) {
        this.gamePhase = gamePhase;
        soldierActionHandler = new SoldierActionHandler(pathingAndLOS, gamePhase);
        playerActionHandler = new PlayerActionHandler(
            input: playerActionInput,
            selectionState: CurrentSelectionState.instance,
            gamePhase: gamePhase,
            soldierActionHandler: soldierActionHandler
        );
    }
}

public class Main : MonoBehaviour {

    public GamePhase gamePhase;
    public UIInputHandler uiInputHandler;
    public Map map;

    GameLogicOrchestrator gameLogicOrchestrator;

    void Awake() {
        gameLogicOrchestrator = new GameLogicOrchestrator(
            gamePhase: gamePhase,
            playerActionInput: uiInputHandler,
            pathingAndLOS: new SoldierPathingAndLOS(map)
        );
    }
}

public class SoldierPathingAndLOS : SoldierActionHandler.IPathingAndLOS, IBlocker, IPathable {

    Map map;

    public SoldierPathingAndLOS(Map map) {
        this.map = map;
    }

    public float Blockage(Vector2 gridLocation) {
        var tile = map.GetTileAt(gridLocation);
        if (!tile.open || tile.foggy) return 1;
        if (tile.GetActor<Soldier>() != null) return 0.35f;
        return 0;
    }

    public bool ValidTarget(Vector2 gridLocation) {
        return !map.GetTileAt(gridLocation).foggy;
    }

    public bool LocationPathable(Vector2 gridLocation) {
        var tile = map.GetTileAt(gridLocation);
        if (tile.actor != null && tile.GetActor<Soldier>() == null) return false;
        return tile.open;
    }

    //

    public bool LOSBlocked(Tile startTile, Tile endTile) {
        return new LineOfSight(startTile.gridLocation, endTile.gridLocation, this).Blocked();
    }

    public Path GetPath(Tile startTile, Tile endTile) {
        var targets = new List<Vector2>() { endTile.gridLocation };
        return new Path(new PathFinder(this, startTile.gridLocation, targets).FindPath());
    }

    public Tile GetTileAt(Vector2 gridLocation) {
        return map.GetTileAt(gridLocation);
    }
}

public class UIInputHandler : PlayerActionHandler.IPlayerActionInput {

    Action turnSoldierLeftListener;
    Action turnSoldierRightListener;
    Action turnSoldierUpListener;
    Action turnSoldierDownListener;
    Action continueButtonListener;
    Action<Tile> interactWithTileListener;

    public override void SetTurnSoldierLeftListener(Action listener) {
        turnSoldierLeftListener = listener;
    }

    public override void SetTurnSoldierRightListener(Action listener) {
        turnSoldierRightListener = listener;
    }

    public override void SetTurnSoldierUpListener(Action listener) {
        turnSoldierUpListener = listener;
    }

    public override void SetTurnSoldierDownListener(Action listener) {
        turnSoldierDownListener = listener;
    }

    public override void SetContinueButtonListener(Action listener) {
        continueButtonListener = listener;
    }

    public override void SetInteractWithTileListener(Action<Tile> listener) {
        interactWithTileListener = listener;
    }

    public void TriggerTurnSoldierLeft() {
        turnSoldierLeftListener();
    }

    public void TriggerTurnSoldierRight() {
        turnSoldierRightListener();
    }

    public void TriggerTurnSoldierUp() {
        turnSoldierUpListener();
    }

    public void TriggerTurnSoldierDown() {
        turnSoldierDownListener();
    }

    public void TriggerContinueButton() {
        continueButtonListener();
    }

    public void TriggerInteractWithTile(Tile tile) {
        interactWithTileListener(tile);
    }
}

public class PlayerActionHandler {

    IPlayerActionInput input;
    CurrentSelectionState selectionState;
    GamePhase gamePhase;
    SoldierActionHandler soldierActionHandler;

    public PlayerActionHandler(
        IPlayerActionInput input,
        CurrentSelectionState selectionState,
        GamePhase gamePhase,
        SoldierActionHandler soldierActionHandler
    ) {
        this.input = input;
        this.selectionState = selectionState;
        this.gamePhase = gamePhase;
        this.soldierActionHandler = soldierActionHandler;
        InitBindings();
    }

    void InitBindings() {
        input.SetTurnSoldierLeftListener(TurnSoldierLeft);
        input.SetTurnSoldierRightListener(TurnSoldierRight);
        input.SetTurnSoldierUpListener(TurnSoldierUp);
        input.SetTurnSoldierDownListener(TurnSoldierDown);
        input.SetContinueButtonListener(ContinueButton);
        input.SetInteractWithTileListener(InteractWithTile);
    }

    void TurnSoldierLeft() {
        if (selectionState.soldierSelected)
            selectionState.GetSelectedSoldier().TurnTo(Soldier.Direction.Left);
    }

    void TurnSoldierRight() {
        if (selectionState.soldierSelected)
            selectionState.GetSelectedSoldier().TurnTo(Soldier.Direction.Right);
    }

    void TurnSoldierUp() {
        if (selectionState.soldierSelected)
            selectionState.GetSelectedSoldier().TurnTo(Soldier.Direction.Up);
    }

    void TurnSoldierDown() {
        if (selectionState.soldierSelected)
            selectionState.GetSelectedSoldier().TurnTo(Soldier.Direction.Down);
    }

    void ContinueButton() {
        gamePhase.ProceedPhase();
    }

    void InteractWithTile(Tile tile) {
        var soldier = tile.GetActor<Soldier>();
        if (soldier != null) {
            selectionState.SelectSoldier(soldier);
        } else {
            if (soldierActionHandler.AnyActionApplicableFor(selectionState.GetSelectedSoldier(), tile)) {
                soldierActionHandler.PerformActionFor(selectionState.GetSelectedSoldier(), tile);
            } else {
                selectionState.DeselectSoldier();
            }
        }
    }

    public abstract class IPlayerActionInput : MonoBehaviour {

        public abstract void SetTurnSoldierLeftListener(Action listener);
        public abstract void SetTurnSoldierRightListener(Action listener);
        public abstract void SetTurnSoldierUpListener(Action listener);
        public abstract void SetTurnSoldierDownListener(Action listener);
        public abstract void SetContinueButtonListener(Action listener);
        public abstract void SetInteractWithTileListener(Action<Tile> listener);
    }
}

public class CurrentSelectionState {

    private static CurrentSelectionState _instance;

    public static CurrentSelectionState instance { get {
        if (_instance == null) _instance = new CurrentSelectionState();
        return _instance;
    } }

    public static void Purge() {
        _instance = null;
    }

    public bool soldierSelected { get { return selectedSoldier != null; } }

    Soldier selectedSoldier;
    UnityEvent SelectionChanged;

    CurrentSelectionState() {
        SelectionChanged = new UnityEvent();
    }

    public void SelectSoldier(Soldier soldier) {
        // Trigger callback
        selectedSoldier = soldier;
    }

    public void DeselectSoldier() {
        // Trigger callback
        selectedSoldier = null;
    }

    public Soldier GetSelectedSoldier() {
        return selectedSoldier;
    }
}

public class SoldierActionHandler {

    IPathingAndLOS pathingAndLOS;
    GamePhase gamePhase;

    public SoldierActionHandler(IPathingAndLOS pathingAndLOS, GamePhase gamePhase) {
        this.pathingAndLOS = pathingAndLOS;
        this.gamePhase = gamePhase;
    }

    public bool AnyActionApplicableFor(Soldier soldier, Tile targetTile) {
        if (soldier == null) return false;
        if (gamePhase.movement) {
            return AnyMovingActionApplicableFor(soldier, targetTile);
        } else {
            return AnyShootingActionApplicableFor(soldier, targetTile);
        }
    }

    public void PerformActionFor(Soldier soldier, Tile targetTile) {
        if (soldier == null) return;
        if (gamePhase.movement) {
            PerformMoveActionFor(soldier, targetTile);
        } else {
            PerformShootingActionFor(soldier, targetTile);
        }
    }

    bool AnyMovingActionApplicableFor(Soldier soldier, Tile targetTile) {
        var path = pathingAndLOS.GetPath(soldier.tile, targetTile);
        return targetTile.open && !targetTile.occupied && path.Count <= soldier.remainingMovement;
    }

    bool AnyShootingActionApplicableFor(Soldier soldier, Tile targetTile) {
        var alien = targetTile.GetActor<Alien>();
        return alien != null && soldier.hasAmmo && soldier.WithinSightArc(targetTile.gridLocation) && !pathingAndLOS.LOSBlocked(soldier.tile, targetTile);
    }

    void PerformMoveActionFor(Soldier soldier, Tile targetTile) {
        if (!AnyMovingActionApplicableFor(soldier, targetTile)) return;
        var path = pathingAndLOS.GetPath(soldier.tile, targetTile);
        soldier.MoveTo(targetTile);
        soldier.TurnTo(targetTile.gridLocation - path.Last());
        TriggerTileWalkedOnEvents(path.nodes, targetTile);
    }

    void PerformShootingActionFor(Soldier soldier, Tile targetTile) {
        if (!AnyShootingActionApplicableFor(soldier, targetTile)) return;
        var alien = targetTile.GetActor<Alien>();
        if (soldier.firesOrdnance) {
            Debug.Log("Firing ordnance");
        } else {
            Debug.Log("Firing gun");
        }
    }

    void TriggerTileWalkedOnEvents(List<Vector2> path, Tile target) {
        for (int i = 1; i < path.Count; i++) {
            pathingAndLOS.GetTileAt(path[i]).SoldierEnter.Invoke();
        }
        target.SoldierEnter.Invoke();
    }

    public interface IPathingAndLOS {

        bool LOSBlocked(Tile startTile, Tile endTile);

        Path GetPath(Tile startTile, Tile endTile);

        Tile GetTileAt(Vector2 gridLocation);
    }
}
