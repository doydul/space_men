namespace Data {
    
    public struct ProgressGamePhaseOutput {
        
        public Data.GamePhase currentPhase;
        public AlienAction[] alienActions;
        public Data.Alien[] newAliens;
        public Position[] radarBlips;
    }

    public struct AlienAction {
        
        public long index;
        public AlienActionType type;
        public Position position;
        public Direction facing;

        public DamageInstance damageInstance;
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
