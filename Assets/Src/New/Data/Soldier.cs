namespace Data {

    public struct Soldier {
        
        public long index;
        public int health;
        public int maxHealth;
        public ArmourType armourType;
        public string weaponName;
        public int exp;
        public int baseMovement;
        public int totalMovement;
        public int moved;
        
        public Position position;
    }

    public enum ArmourType {
        Basic,
        Heavy,
        Medium
    }

    public enum ArmourWeight {
        Light,
        Medium,
        Heavy
    }

    public struct ArmourStats {
        public ArmourWeight weight;
        public int armourValue;
        public int cost;
    }
}
