namespace Data {
    
    public struct ProgressGamePhaseOutput {
        
        public GamePhase currentPhase;
        public AlienAction[] alienActions;
    }

    public struct AlienAction {

        public AlienActionType type;
        public Position position;
        public int damage;
        public AttackResult attackResult;
    }

    public enum AlienActionType {
        Move,
        Attack
    }

    public enum AttackResult {
        Hit,
        Missed,
        Deflected
    }
}
