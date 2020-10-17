using UnityEngine;
using System;

using Data;

public class PlayerActionHandler {

    public static PlayerActionHandler instance { get; private set; }

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
        instance = this;
    }

    public void InteractWithTile(Tile tile) {
        var actor = tile.GetActor<Actor>();
        ActorAction action;
        ShipAction shipAction;
        if (UIData.instance.ActionFor(tile, out action)) {
            if (action.type == ActorActionType.Move) {
                MapController.instance.MoveSoldier(action.index, tile.gridLocation);
            } else if (action.type == ActorActionType.Shoot) {
                MapController.instance.SoldierShoot(action.index, action.actorTargetIndex);
            } else if (action.type == ActorActionType.Special) {
                MapController.instance.PerformSpecialAction(tile, action.specialAction);
            }
        } else if (UIData.instance.ShipActionFor(tile, out shipAction)) {
            MapController.instance.PerformShipAction(tile, shipAction.type);
        } else if (actor != null) {
            UIData.instance.selectedTile = tile;
            Header.instance.Display(actor);
            MapController.instance.DisplayActions(actor.index);
            MapHighlighter.instance.HighlightTile(tile, Color.white);
            if (actor is Soldier) {
                Scripting.instance.Trigger(Scripting.Event.OnSelectSoldier);
            }
        } else {
            TurnButtons.instance.Hide();
            Header.instance.Hide();
            selectionState.DeselectSoldier();
            UIData.instance.ClearSelection();
            MapHighlighter.instance.ClearHighlights();
            WorldButtonsContainer.instance.HideAll();
            SpecialAbilityPanel.instance.Close();
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
