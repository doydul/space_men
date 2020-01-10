using System;
using System.Collections.Generic;

using Interactors;
using Workers;

public class MissionOverviewInitializer : InitializerBase {

    protected override void GenerateControllerMapping() {
        controllerMapping.Add(typeof(MissionOverviewController), new Dictionary<Type, Type> {
            { typeof(OpenMissionOverviewInteractor), typeof(OpenMissionOverviewPresenter) },
            { typeof(LoadMissionInteractor), typeof(LoadMissionPresenter) }
        });
    }

    protected override void GenerateDependencies() {
        dependencies.Add(new MissionStore());
    }

    protected override void Initialize() {
        FindObjectOfType<MissionOverviewController>().InitializeMissionOverview();
    }
}
