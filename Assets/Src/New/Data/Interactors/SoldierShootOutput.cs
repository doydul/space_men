namespace Data {
    
    public struct SoldierShootOutput {
        
        public long soldierIndex;
        public int shotsLeft;
        public int ammoLeft;
        public int maxAmmo;
        public string weaponName;
        public DamageInstance[] damageInstances;
        public ExplosionData? explosion;
    }

    public struct DamageInstance {

        public int damageInflicted;
        public AttackResult attackResult;
        public bool critical;
        public long perpetratorIndex;
        public long victimIndex;
        public int victimHealthAfterDamage;
    }
}
