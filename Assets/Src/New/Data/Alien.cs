namespace Data { 

    public struct Alien {
        
        public long index;
        public AlienType alienType;
        public Position position;
        public Direction facing;
    }

    public enum AlienType {
        Alien,
        Advanced
    }
    
    public struct AlienStats {
        public int health;
        public int armour;
        public int accModifier;
        public int damage;
        public int armourPen;
        public int movement;
    }
}
