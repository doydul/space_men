namespace Data {

    public struct Soldier {
        
        public int health;
        public int maxHealth;
        public string armourName;
        public string weaponName;
        public int exp;
        public int baseMovement;
        public int totalMovement;
        public int moved;
        public int remainingMovement { get { return totalMovement - moved; } }
        
        public Position position;
    }
}
