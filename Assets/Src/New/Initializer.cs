using UnityEngine;
using System;
using System.Reflection;
using System.Collections.Generic;

using Interactors;
using Workers;

public class Initializer : MonoBehaviour {
    
    static Dictionary<Type, Dictionary<Type, Type>> controllerInteractorPresenterMapping = 
        new Dictionary<Type, Dictionary<Type, Type>> {
            { typeof(UIController),
                new Dictionary<Type, Type> {
                    { typeof(ProgressGamePhaseInteractor), typeof(ProgressGamePhasePresenter) }
                }
            },
            { typeof(MapController),
                new Dictionary<Type, Type> {
                    { typeof(MissionStartInteractor), typeof(MissionStartPresenter) },
                    { typeof(MoveSoldierInteractor), typeof(MoveSoldierPresenter) },
                    { typeof(ActorActionsInteractor), typeof(ActorActionsPresenter) },
                    { typeof(TurnSoldierInteractor), typeof(TurnSoldierPresenter) },
                    { typeof(SoldierShootInteractor), typeof(SoldierShootPresenter) }
                }
            },
            { typeof(ScriptingController),
                new Dictionary<Type, Type> {
                    { typeof(FinishMissionInteractor), typeof(FinishMissionPresenter) },
                    { typeof(CompleteSecondaryMissionInteractor), typeof(CompleteSecondaryMissionPresenter) }
                }
            },
            { typeof(MetaGameController),
                new Dictionary<Type, Type> {
                    // { typeof(DoSomeActionInteractor), typeof(DoSomeActionPresenter) }
                }
            }
        };
    
    static List<object> dependencies;
    
    GameState gameState;
    
    void Start() {
        MetaGameState.Init();

        gameState = new GameState();
        var mapStore = new MapStore();
        mapStore.map = Map.instance;
        gameState.mapStore = mapStore;
        gameState.Init("Default", "First Mission"); // TODO: load from metagame state

        dependencies = new List<object>();
        dependencies.Add(gameState);
        dependencies.Add(new AlienStore());
        dependencies.Add(new SoldierStore());
        dependencies.Add(new MissionStore());
        dependencies.Add(MetaGameState.instance);
        
        LoadDynamicDependencies();
        
        FindObjectOfType<MapController>().StartMission();
    }

    void LoadDynamicDependencies() {
        foreach (var controllerType in controllerInteractorPresenterMapping) {
            var controller = FindObjectOfType(controllerType.Key);
            foreach (var interactorType in controllerType.Value) {
                var interactor = Activator.CreateInstance(interactorType.Key);
                if (interactorType.Value != null) { 
                    var presprop = interactorType.Key.GetProperty("presenter");
                    var presenter = FindObjectOfType(interactorType.Value);
                    presprop.SetValue(interactor, presenter);
                }
                foreach (var property in interactorType.Key.GetProperties()) {
                    foreach (var dependency in dependencies) {
                        if (property.PropertyType.IsAssignableFrom(dependency.GetType())) {
                            property.SetValue(interactor, dependency);
                            break;
                        }
                    }
                }
                foreach (var property in controllerType.Key.GetProperties()) {
                    if (property.PropertyType == interactorType.Key) {
                        property.SetValue(controller, interactor);
                        break;
                    }
                }
            }
        }
    }
}
