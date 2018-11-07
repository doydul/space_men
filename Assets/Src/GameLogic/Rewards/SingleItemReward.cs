public class SingleItemReward : IUnclaimedMissionReward {

    private InventoryItem item;

    public SingleItemReward(InventoryItem item) {
        this.item = item;
    }

    public void Claim(RewardClaimer rewardClaimer) {
        rewardClaimer.Claim(this);
    }
}
