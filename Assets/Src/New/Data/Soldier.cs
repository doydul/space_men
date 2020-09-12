namespace Data {

    public struct Soldier {
        
        public long index;
        public int health;
        public int maxHealth;
        public string armourName;
        public string weaponName;
        public int exp;
        public int moved;
        public Direction facing;
        
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
        public int maxHealth;
        public int movement;
        public int sprint;

        public bool isNull => cost == -1;
    }

    public struct WeaponStats {
        public int accuracy;
        public int armourPen;
        public int cost;
        public int minDamage;
        public int maxDamage;
        public int shotsWhenMoving;
        public int shotsWhenStill;
        public float blast;

        public bool isNull => cost == -1;
    }
}
