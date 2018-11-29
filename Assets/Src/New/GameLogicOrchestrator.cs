using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

public class GameLogicOrchestrator {

    public GameLogicOrchestrator(
        GamePhase gamePhase,
        PlayerActionHandler.IPlayerActionInput playerActionInput,
        SoldierActionHandler.IPathingAndLOS pathingAndLOS,
        IGameMap gameMap,
        ICameraController cameraController,
        IWorld world,
        Map legacyMap
    ) {
        GameAction.SetShootAction(new ShootAction(world, new Exploder(world)));
        var alienDeployer = new AlienDeployer(legacyMap, gamePhase);
        var alienMovementPhaseDirector = new AlienMovementPhaseDirector(
            legacyMap,
            cameraController
        );
        var radarBlipController = new RadarBlipController(alienDeployer);
        GamePhase.phaseFactory = new PhaseFactory(
            legacyMap,
            alienMovementPhaseDirector,
            alienDeployer,
            radarBlipController
        );
        var fogController = new FogController(gameMap, new FogChanged(alienDeployer, radarBlipController));
        var currentSelectionState = new CurrentSelectionState();
        ViewableState.Init(gamePhase, currentSelectionState);
        var soldierActionHandler = new SoldierActionHandler(
            pathingAndLOS,
            gamePhase,
            new SoldierMoved(fogController)
        );
        var playerActionHandler = new PlayerActionHandler(
            input: playerActionInput,
            selectionState: currentSelectionState,
            gamePhase: gamePhase,
            soldierActionHandler: soldierActionHandler
        );
        playerActionHandler.InitBindings();
        new GameInitializer(legacyMap, fogController).Init();
    }
}
