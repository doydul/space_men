using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class InitializerBase : MonoBehaviour {
    
    protected Dictionary<Type, Dictionary<Type, Type>> controllerMapping;
    protected List<object> dependencies;

    protected abstract void GenerateControllerMapping();
    protected abstract void GenerateDependencies();
    protected abstract void Initialize();
    
    void Start() {
        dependencies = new List<object>();
        controllerMapping = new Dictionary<Type, Dictionary<Type, Type>>();
        GenerateControllerMapping();
        GenerateDependencies();
        LoadDynamicDependencies();
        Initialize();
    }

    void LoadDynamicDependencies() {
        foreach (var controllerType in controllerMapping) {
            var controller = FindObjectOfType(controllerType.Key);
            foreach (var interactorType in controllerType.Value) {
                var interactor = Activator.CreateInstance(interactorType.Key);
                if (interactorType.Value != null) { 
                    var presprop = interactorType.Key.GetProperty("presenter");
                    var presenter = FindObjectOfType(interactorType.Value);
                    foreach (var property in interactorType.Value.GetProperties()) {
                        foreach (var dependency in dependencies) {
                            if (property.PropertyType.IsAssignableFrom(dependency.GetType())) {
                                property.SetValue(presenter, dependency);
                                break;
                            }
                        }
                    }
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
