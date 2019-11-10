namespace Data { 

    public struct Alien {
        
        public long index;
        public string alienType;
        public Position position;
        public Direction facing;
    }
    
    public struct AlienStats {
        public int health;
        public int armour;
        public int accModifier;
        public int damage;
        public int armourPen;
        public int movement;
        public int radarBlipChance;
        public int maxHealth;
        public string name;
    }
}
