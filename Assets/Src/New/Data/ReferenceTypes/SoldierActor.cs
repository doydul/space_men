namespace Data { 
  
    public class SoldierActor : Actor {

        public override bool isSoldier { get { return true; } }

        public string armourName { get; set; }
        public string weaponName { get; set; }
        public int exp { get; set; }
        public int baseMovement { get; set; }
        public int totalMovement { get; set; }
        public int moved { get; set; }
        public int ammoSpent { get; set; }
        public int remainingMovement { get { return totalMovement - moved; } }
        
        public Data.Soldier ToValueType() {
            return new Data.Soldier {
                index = uniqueId,
                health = health.current,
                maxHealth = health.max,
                armourName = armourName,
                weaponName = weaponName,
                exp = exp,
                baseMovement = baseMovement,
                totalMovement = totalMovement,
                position = position
            };
        }
    }
}