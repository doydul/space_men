using Workers;

namespace Data {

    public class ArmourReward : IReward {

        string armourName;

        public ArmourReward(string armourName) {
            this.armourName = armourName;
        }

        public void Grant(MetaGameState metaGameState) {
            metaGameState.metaItems.AddInventoryItem(new MetaArmour {
                name = armourName
            });
        }
    }
}