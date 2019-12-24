using System;
using System.Collections.Generic;

using Interactors;
using Workers;

public class Initializer : InitializerBase {

    protected override void GenerateControllerMapping() {
        controllerMapping.Add(typeof(UIController), new Dictionary<Type, Type> {
            { typeof(ProgressGamePhaseInteractor), typeof(ProgressGamePhasePresenter) }
        });
        controllerMapping.Add(typeof(MapController), new Dictionary<Type, Type> {
            { typeof(MissionStartInteractor), typeof(MissionStartPresenter) },
            { typeof(MoveSoldierInteractor), typeof(MoveSoldierPresenter) },
            { typeof(ActorActionsInteractor), typeof(ActorActionsPresenter) },
            { typeof(TurnSoldierInteractor), typeof(TurnSoldierPresenter) },
            { typeof(SoldierShootInteractor), typeof(SoldierShootPresenter) }
        });
        controllerMapping.Add(typeof(ScriptingController), new Dictionary<Type, Type> {
            { typeof(FinishMissionInteractor), typeof(FinishMissionPresenter) },
            { typeof(CompleteSecondaryMissionInteractor), typeof(CompleteSecondaryMissionPresenter) }
        });
        // controllerMapping.Add(typeof(MetaGameController), new Dictionary<Type, Type> {
            
        // });
    }

    protected override void GenerateDependencies() {
        MetaGameState.Load(0, new MetaGameSave {
            credits = 100,
            soldiers = new MetaSoldierSave[0],
            items = new MetaItemSave[0],
            currentCampaign = "Default",
            currentMission = "First Mission"
        });
        var gameState = new GameState();
        var mapStore = new MapStore();
        mapStore.map = Map.instance;
        gameState.mapStore = mapStore;
        gameState.Init(MetaGameState.instance.currentCampaign, MetaGameState.instance.currentMission);

        dependencies.Add(gameState);
        dependencies.Add(new AlienStore());
        dependencies.Add(new SoldierStore());
        dependencies.Add(new MissionStore());
        dependencies.Add(MetaGameState.instance);
    }

    protected override void Initialize() {
        FindObjectOfType<MapController>().StartMission();
    }
}
