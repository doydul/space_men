using UnityEngine;
using System;

public class PlayerActionHandler {

    IPlayerActionInput input;
    CurrentSelectionState selectionState;
    GamePhase gamePhase;
    SoldierActionHandler soldierActionHandler;
    UIController uiController;

    public PlayerActionHandler(
        IPlayerActionInput input,
        CurrentSelectionState selectionState,
        GamePhase gamePhase,
        SoldierActionHandler soldierActionHandler,
        UIController uiController
    ) {
        this.input = input;
        this.selectionState = selectionState;
        this.gamePhase = gamePhase;
        this.soldierActionHandler = soldierActionHandler;
        this.uiController = uiController;
    }

    public void InitBindings() {
        input.SetTurnSoldierLeftListener(TurnSoldierLeft);
        input.SetTurnSoldierRightListener(TurnSoldierRight);
        input.SetTurnSoldierUpListener(TurnSoldierUp);
        input.SetTurnSoldierDownListener(TurnSoldierDown);
        input.SetContinueButtonListener(ContinueButton);
        input.SetInteractWithTileListener(InteractWithTile);
        input.SetInfoButtonListener(InfoButton);
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
    
    void InfoButton(bool open) {
        if (open) { 
            if (selectionState.soldierSelected) {
                uiController.InspectSoldier(selectionState.GetSelectedSoldier().index);
            }
        } else {
            uiController.CloseInfoPanel();
        }
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
        public abstract void SetInfoButtonListener(Action<bool> listener);
    }
}
