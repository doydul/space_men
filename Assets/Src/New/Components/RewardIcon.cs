using UnityEngine;
using UnityEngine.UI;

public class RewardIcon : MonoBehaviour {
    
    public MissionReward reward { private get; set; }

    public Image rewardImage;
    public Sprite creditsIconSpite;

    void Start() {
        if (reward.type == MissionReward.Type.Credits) {
            rewardImage.sprite = creditsIconSpite;
        }
    }
}