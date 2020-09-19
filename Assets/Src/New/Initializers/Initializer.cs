using System;
using System.Collections.Generic;

using Interactors;
using Workers;

public class Initializer : InitializerBase {

    protected override void GenerateControllerMapping() {
        controllerMapping.Add(typeof(UIController), new Dictionary<Type, Type> {
            { typeof(ProgressGamePhaseInteractor), typeof(ProgressGamePhasePresenter) },
            { typeof(OpenShipAbilitiesInteractor), typeof(OpenShipAbilitiesPresenter) },
            { typeof(DisplayShipAbilityTargetsInteractor), typeof(DisplayShipAbilityTargetsPresenter) }
        });
        controllerMapping.Add(typeof(MapController), new Dictionary<Type, Type> {
            { typeof(MissionStartInteractor), typeof(MissionStartPresenter) },
            { typeof(MoveSoldierInteractor), typeof(MoveSoldierPresenter) },
            { typeof(ActorActionsInteractor), typeof(ActorActionsPresenter) },
            { typeof(TurnSoldierInteractor), typeof(TurnSoldierPresenter) },
            { typeof(SoldierShootInteractor), typeof(SoldierShootPresenter) },
            { typeof(ExecuteShipAbilityInteractor), typeof(ExecuteShipAbilityPresenter) },
            { typeof(CollectAmmoInteractor), typeof(CollectAmmoPresenter) }
        });
        controllerMapping.Add(typeof(ScriptingController), new Dictionary<Type, Type> {
            { typeof(FinishMissionInteractor), typeof(FinishMissionPresenter) },
            { typeof(CompleteSecondaryMissionInteractor), typeof(CompleteSecondaryMissionPresenter) },
            { typeof(SpawnAliensInteractor), typeof(SpawnAliensPresenter) }
        });
        // controllerMapping.Add(typeof(MetaGameController), new Dictionary<Type, Type> {
            
        // });
    }

    protected override void GenerateDependencies() {
        if (MetaGameState.instance == null) {
            MetaGameState.Load(0, DebugSaveGenerator.Generate());
        }

        var gameState = new GameState();
        var mapStore = new MapStore();
        mapStore.map = Map.instance;
        gameState.mapStore = mapStore;
        gameState.Init();

        // dependencies.Add(gameState);
        dependencies.Add(new AlienStore());
        dependencies.Add(new SoldierStore());
        dependencies.Add(new MissionStore());
        dependencies.Add(new CampaignStore());

        factory.RegisterDependency(typeof(GameState), gameState);
        factory.RegisterDependency(typeof(MetaGameState), MetaGameState.instance);
        factory.RegisterDependency(typeof(ISoldierStore), new SoldierStore());
    }

    protected override void Initialize() {
        FindObjectOfType<MapController>().StartMission();
    }
}
