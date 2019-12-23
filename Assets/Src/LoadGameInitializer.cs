using System;
using System.Collections.Generic;

using Interactors;
using Workers;

public class LoadGameInitializer : InitializerBase {

    protected override void GenerateControllerMapping() {
        controllerMapping.Add(typeof(LoadGameController), new Dictionary<Type, Type> {
            { typeof(OpenLoadingMenuInteractor), typeof(OpenLoadingMenuPresenter) },
            { typeof(LoadGameInteractor), typeof(LoadGamePresenter) }
        });
    }

    protected override void GenerateDependencies() {
        dependencies.Add(new MetaGameStateStore());
    }

    protected override void Initialize() {
        FindObjectOfType<LoadGameController>().InitializeLoadGamePage();
    }
}
