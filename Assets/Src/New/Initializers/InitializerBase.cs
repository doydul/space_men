using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class InitializerBase : MonoBehaviour {
    
    protected Dictionary<Type, Dictionary<Type, Type>> controllerMapping;
    protected List<object> dependencies;
    protected DependencyInjector factory;

    protected abstract void GenerateControllerMapping();
    protected abstract void GenerateDependencies();
    protected abstract void Initialize();
    
    void Start() {
        dependencies = new List<object>();
        factory = new DependencyInjector();
        controllerMapping = new Dictionary<Type, Dictionary<Type, Type>>();
        GenerateControllerMapping();
        GenerateDependencies();
        LoadDynamicDependencies();
        Initialize();
    }

    void LoadDynamicDependencies() {
        var presenters = FindObjectsOfType<Presenter>();
        foreach (var presenter in presenters) {
            factory.InjectDependencies(presenter);
            factory.RegisterDependency(presenter.GetType().GetInterfaces()[0], presenter);
        }
        foreach (var controllerType in controllerMapping) {
            var controller = FindObjectOfType(controllerType.Key);
            foreach (var interactorType in controllerType.Value) {
                var interactor = factory.MakeObject(interactorType.Key);
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

        var controllers = FindObjectsOfType<Controller>();
        foreach (var controller in controllers) {
            factory.InjectDependencies(controller);
        }
    }
}
