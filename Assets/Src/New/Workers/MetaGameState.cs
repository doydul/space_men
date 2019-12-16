using DataTypes;

namespace Workers {

    public class MetaGameState {

        public static MetaGameState instance { get; private set; }

        public static void Init() {
            instance = new MetaGameState(); // TODO: Will need to load from persistant state at some point
        }

        public MetaGameState() {
            metaSoldiers = new MetaSoldiers(); // TODO: get from persistant state
            metaItems = new MetaItems(); // TODO: get from persistant state
            credits = new Credits(100); // TODO: get from persistant state
        }

        public MetaSoldiers metaSoldiers { get; private set; }
        public MetaItems metaItems { get; private set; }
        public Credits credits { get; private set; }
        public string currentCampaign { get; private set; }
        public string currentMission { get; private set; }
    }
}
