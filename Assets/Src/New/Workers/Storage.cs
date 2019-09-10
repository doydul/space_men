using Data;

namespace Workers {
    
    public class Storage {
        
        static IMissionStore missionStore;
        
        public static Storage instance { get; private set; }
        
        public static void Init(IMissionStore missionStore) {
            Storage.missionStore = missionStore;
            instance = new Storage();
        }
        
        string currentCampaign = "Default";
        string currentMission = "First Mission";
        
        Data.GamePhase currentPhase;
        
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
        
        public Data.Mission GetMission(string campaignName, string missionName) {
            return missionStore.GetMission(campaignName, missionName);
        }
        
        public Data.Mission GetCurrentMission() {
            return GetMission(currentCampaign, currentMission);
        }
        
        public Data.GamePhase GetCurrentPhase() {
            return currentPhase;
        }

        public void SetCurrentPhase(Data.GamePhase value) {
            currentPhase = value;
        }
        
        public void UpdateSoldier(int soldierIndex, Data.Soldier newValue) {
            // TODO when this is no longer just a hack
        }
    }
}
