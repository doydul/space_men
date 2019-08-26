namespace Workers {
    
    public class Storage {
        
        static Storage _instance;
        
        public static Storage instance { get {
            if (_instance == null) Init();
            return _instance;
        } }
        
        static void Init() {
            _instance = new Storage();
        }
        
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
    }
}
