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
                    { typeof(ProgressGamePhaseInteractor), null }
                }
            },
            { typeof(MapController),
                new Dictionary<Type, Type> {
                    { typeof(SoldierPossibleMovesInteractor), typeof(SoldierPossibleMovesPresenter) }
                }
            }
        };
    
    void Awake() {
        Storage.Init(new MissionStore());
        
        foreach (var controllerType in controllerInteractorPresenterMapping) {
            var controller = FindObjectOfType(controllerType.Key);
            foreach (var interactorType in controllerType.Value) {
                var interactor = Activator.CreateInstance(interactorType.Key);
                if (interactorType.Value != null) { 
                    var prop = interactorType.Key.GetProperty("presenter");
                    var presenter = FindObjectOfType(interactorType.Value);
                    prop.SetValue(interactor, presenter);
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
