using UnityEngine;
using UnityEngine.UI;

public class RewardIcon : MonoBehaviour {
    
    public MissionReward reward { private get; set; }

    public Icon rewardIcon;

    void Start() {
        if (reward.type == MissionReward.Type.Credits) {
            rewardIcon.Init(reward.credits);
        } else if (reward.type == MissionReward.Type.Weapon) {
            rewardIcon.Init(Weapon.Get(reward.itemName));
        } else if (reward.type == MissionReward.Type.Armour) {
            rewardIcon.Init(Armour.Get(reward.itemName));
        } else if (reward.type == MissionReward.Type.Soldier) {
            rewardIcon.Init();
        }
    }
}