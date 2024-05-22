using DataTypes;
using System.Collections.Generic;
using System.Linq;

namespace Workers {

    public class MetaGameState {

        public static MetaGameState instance { get; private set; }

        public static void Load(int slot, MetaGameSave save) {
        }

        public static void Unload() {
            instance = null;
        }

        public MetaGameState() {
            metaSoldiers = new MetaSoldiers();
            metaItems = new MetaItems();
            credits = new Credits(0);
        }

        public int saveSlot { get; private set; }
        public MetaSoldiers metaSoldiers { get; private set; }
        public MetaItems metaItems { get; private set; }
        public Credits credits { get; private set; }
        public string currentCampaign { get; private set; }
        public string currentMission { get; set; }
    }
}
