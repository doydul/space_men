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
        if (actor != null) {
            UIData.instance.selectedTile = tile;
            MapController.instance.DisplayActions(actor.index);
        } else {
            ActorAction action;
            if (UIData.instance.ActionFor(tile, out action)) {
                if (action.type == ActorActionType.Move) {
                    MapController.instance.MoveSoldier(action.index, tile.gridLocation);
                } else if (action.type == ActorActionType.Shoot) {
                    MapController.instance.SoldierShoot(action.index, action.actorTargetIndex);
                }
            } else {
                selectionState.DeselectSoldier();
                UIData.instance.ClearSelection();
                MapHighlighter.instance.ClearHighlights();
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
