namespace Data { 
  
    public class SoldierActor : Actor {

        public override bool isSoldier { get { return true; } }

        public long metaSoldierId { get; set; }
        public string armourName { get; set; }
        public string weaponName { get; set; }
        public int exp { get; set; }
        public int moved { get; set; }
        public int shotsFiredThisTurn { get; set; }
        public int ammoSpent { get; set; }
        
        public Data.Soldier ToValueType() {
            return new Data.Soldier {
                index = uniqueId,
                health = health.current,
                maxHealth = health.max,
                armourName = armourName,
                weaponName = weaponName,
                exp = exp,
                position = position,
                facing = facing
            };
        }
    }
}
