namespace Data {
    
    public struct SoldierShootOutput {
        
        public long soldierIndex;
        public int ammoSpent;
        public string weaponName;
        public DamageInstance[] damageInstances;
        public Position[] blastCoverage;
    }

    public struct DamageInstance {

        public int damageInflicted;
        public AttackResult attackResult;
        public long perpetratorIndex;
        public long victimIndex;
    }
}
