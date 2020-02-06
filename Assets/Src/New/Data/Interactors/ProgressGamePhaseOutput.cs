namespace Data {
    
    public struct ProgressGamePhaseOutput {
        
        public Data.GamePhase currentPhase;
        public AlienAction[] alienActions;
        public Data.Alien[] newAliens;
        public Position[] radarBlips;
        public ShootingStats[] shootingStats;
    }

    public struct AlienAction {
        public int thing;
        public long index;
        public AlienActionType type;
        public Position position;
        public Direction facing;

        public DamageInstance damageInstance;
    }

    public struct ShootingStats {
        public long soldierID;
        public int shots;
    }

    public enum AlienActionType {
        Move,
        Attack
    }

    public enum AttackResult {
        Hit,
        Missed,
        Deflected,
        Killed
    }
}
