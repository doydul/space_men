namespace Data {
    
    public struct ProgressGamePhaseOutput {
        
        public Data.GamePhase currentPhase;
        public int currentPart;
        public int threatCountdown;
        public int currentThreatLevel;
        public AlienAction[] alienActions;
        public Data.Alien[] newAliens;
        public Position[] radarBlips;
        public ShootingStats[] shootingStats;
        public ShipEnergyEvent? shipEnergyEvent;
        public long[] deadActorIndexes;
        public DamageInstance[] damageInstances;
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

    public struct ShipEnergyEvent {
        public int netChange;
    }

    public enum AlienActionType {
        Move,
        Attack
    }

    public enum AttackResult {
        Hit,
        CriticalHit,
        Missed,
        Deflected,
        Killed
    }
}
