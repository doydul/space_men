using System;
using System.Collections.Generic;

using Interactors;
using Workers;

public class MissionCompleteInitializer : InitializerBase {

    static MissionReward[] missionRewards;

    public MissionCompletePanel missionCompletePanel;

    public static void SetRewards(MissionReward[] rewards) {
        missionRewards = rewards;
    }

    protected override void GenerateControllerMapping() {
        controllerMapping.Add(typeof(MissionCompleteController), new Dictionary<Type, Type> {
            
        });
    }

    protected override void GenerateDependencies() {
        
    }

    protected override void Initialize() {
        missionCompletePanel.PopulateRewardIcons(missionRewards);
    }
}
