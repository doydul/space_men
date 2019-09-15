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
                    { typeof(InspectSelectedItemInteractor), typeof(SelectedItemInfoPresenter) },
                    { typeof(ProgressGamePhaseInteractor), typeof(ProgressGamePhasePresenter) }
                }
            },
            { typeof(MapController),
                new Dictionary<Type, Type> {
                    { typeof(SoldierPossibleMovesInteractor), typeof(SoldierPossibleMovesPresenter) },
                    { typeof(MissionStartInteractor), typeof(MissionStartPresenter) },
                    { typeof(MoveSoldierInteractor), typeof(MoveSoldierPresenter) }
                }
            }
        };
    
    GameState gameState;
    
    void Start() {
        Storage.Init(new MissionStore());

        gameState = new GameState();
        var mapStore = new MapStore();
        mapStore.map = Map.instance;
        gameState.mapStore = mapStore;
        gameState.Init();
        
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

                    var gamestateprop = interactorType.Key.GetProperty("gameState");
                    gamestateprop.SetValue(interactor, gameState);
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
