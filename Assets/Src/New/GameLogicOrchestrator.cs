using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

public class GameLogicOrchestrator {

    GamePhase gamePhase;
    PlayerActionHandler playerActionHandler;
    SoldierActionHandler soldierActionHandler;
    CurrentSelectionState currentSelectionState;

    public GameLogicOrchestrator(GamePhase gamePhase, PlayerActionHandler.IPlayerActionInput playerActionInput, SoldierActionHandler.IPathingAndLOS pathingAndLOS) {
        this.gamePhase = gamePhase;
        soldierActionHandler = new SoldierActionHandler(pathingAndLOS, gamePhase);
        currentSelectionState = new CurrentSelectionState();
        playerActionHandler = new PlayerActionHandler(
            input: playerActionInput,
            selectionState: currentSelectionState,
            gamePhase: gamePhase,
            soldierActionHandler: soldierActionHandler
        );
        ViewableState.Init(gamePhase, currentSelectionState);
    }
}
