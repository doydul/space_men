using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Data;

public class FinishMissionPresenter : Presenter, IPresenter<FinishMissionOutput> {
  
    public static FinishMissionPresenter instance { get; private set; }

    public GameObject victoryPopup;
    
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
        var mission = campaign.missions.First(m => m.missionName == input.missionName);
        result.AddRange(mission.rewards);
        foreach (var completedSecondaryObjectiveId in input.completedSecondaryObjectIds) {
            result.AddRange(mission.secondaryMissions[completedSecondaryObjectiveId].rewards);
        }
        return result.ToArray();
    }
}

