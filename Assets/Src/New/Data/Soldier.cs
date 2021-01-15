namespace Data {

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
        public int ammo;
        public float blast;
        public string description;

        public bool isNull => cost == -1;
    }
}
