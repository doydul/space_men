using UnityEngine;

public class MissionCompletePanel : MonoBehaviour {
    
    public Transform rewardIconContainer;
    public Transform rewardIconPrefab;

    public void PopulateRewardIcons(MissionReward[] missionRewards) {
        foreach (var missionReward in missionRewards) {
            InstantiateMissionReward(missionReward);
        }
    }

    void InstantiateMissionReward(MissionReward missionReward) {
        var rewardIconTransform = Instantiate(rewardIconPrefab) as Transform;
        var rewardIcon = rewardIconTransform.GetComponent<RewardIcon>();
        rewardIconTransform.SetParent(rewardIconContainer, false);
        rewardIcon.reward = missionReward;
    }
}