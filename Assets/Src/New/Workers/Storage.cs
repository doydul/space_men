using Data;

namespace Workers {
    
    public class Storage {
        
        static Storage _instance;
        static IMissionStore missionStore;
        
        public static Storage instance { get {
            if (_instance == null) _instance = new Storage();
            return _instance;
        } }
        
        public static void Init(IMissionStore missionStore) {
            Storage.missionStore = missionStore;
        }
        
        string currentCampaign = "Default";
        string currentMission = "Mission1";
        
        GamePhase currentPhase;
        
        public Data.Soldier GetSoldier(int index) {
            var soldier = SoldierDataHack.soldiers[index];
            return new Data.Soldier {
                health = soldier.health,
                maxHealth = soldier.maxHealth,
                armourName = soldier.armourName,
                weaponName = soldier.weaponName,
                exp = soldier.exp,
                baseMovement = soldier.baseMovement,
                totalMovement = soldier.totalMovement,
                moved = soldier.tilesMoved,
                position = new Data.Position { x = (int)soldier.gridLocation.x, y = (int)soldier.gridLocation.y}
            };
        }

        public Data.Map GetMap() {
            return MapHack.GetData();
        }
        
        public Data.Mission GetMission(string campaignName, string missionName) {
            return IMissionStore.GetMission(campaignName, missionName);
        }
        
        public Data.Mission GetCurrentMission() {
            return GetMission(currentCampaign, currentMission);
        }
        
        public GamePhase GetCurrentPhase() {
            return currentPhase;
        }

        public void SetCurrentPhase(GamePhase value) {
            currentPhase = value;
        }
    }
}
