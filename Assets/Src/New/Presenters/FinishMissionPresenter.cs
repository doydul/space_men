using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Data;

public class FinishMissionPresenter : Presenter, IPresenter<FinishMissionOutput> {
  
    public static FinishMissionPresenter instance { get; private set; }

    public GameObject victoryPopup;
    public Mission mission;
    
    void Awake() {
        instance = this;
    }
    
    public void Present(FinishMissionOutput input) {
        MissionCompleteInitializer.SetRewards(GetRewards(input));
        victoryPopup.SetActive(true);
    }

    MissionReward[] GetRewards(FinishMissionOutput input) {
        var result = new List<MissionReward>();
        var campaign = Campaign.FromString(input.campaignName);
        result.AddRange(mission.rewards);
        foreach (var completedSecondaryObjectiveId in input.completedSecondaryObjectIds) {
            result.AddRange(mission.secondaryMissions[completedSecondaryObjectiveId].rewards);
        }
        return result.ToArray();
    }
}

