using Workers;

namespace Data {

    public class CreditReward : IReward {

        int credits;

        public CreditReward(int credits) {
            this.credits = credits;
        }

        public void Grant(MetaGameState metaGameState) {
            metaGameState.credits.Add(credits);
        }
    }
}