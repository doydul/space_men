using System;
using System.Collections.Generic;

using Interactors;
using Workers;

public class WorkshopInitializer : InitializerBase {

    protected override void GenerateControllerMapping() {
        controllerMapping.Add(typeof(WorkshopController), new Dictionary<Type, Type> {
            { typeof(OpenWorkshopInteractor), typeof(OpenWorkshopPresenter) },
            { typeof(AnalyseItemInteractor), typeof(AnalyseItemPresenter) },
            { typeof(ScrapItemInteractor), typeof(ScrapItemPresenter) },
            { typeof(BuildItemInteractor), typeof(BuildItemPresenter) }
        });
    }

    protected override void GenerateDependencies() {
        if (MetaGameState.instance == null) {
            MetaGameState.Load(0, DebugSaveGenerator.Generate());
        }

        dependencies.Add(new SoldierStore());
    }

    protected override void Initialize() {
        FindObjectOfType<WorkshopController>().InitPage();
    }
}
