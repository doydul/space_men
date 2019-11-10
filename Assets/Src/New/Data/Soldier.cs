namespace Data {

    public struct Soldier {
        
        public long index;
        public int health;
        public int maxHealth;
        public string armourName;
        public string weaponName;
        public int exp;
        public int baseMovement;
        public int totalMovement;
        public int moved;
        
        public Position position;
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

    public struct WeaponStats {
        public int accuracy;
        public int armourPen;
        public int minDamage;
        public int maxDamage;
        public int shotsWhenMoving;
        public int shotsWhenStill;
        public float blast;
        public int value;
    }
}
