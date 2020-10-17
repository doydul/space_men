namespace Data {

    public struct SpecialAbilityOutput {

        public long soldierIndex;
        public int shotsLeft;
        public int ammoLeft;
        public int maxAmmo;
        public string weaponName;
        public DamageInstance[] damageInstances;
        public Position[] blastCoverage;

        public int maxAmmoCount;
        public int newAmmoCount;
        public int remainingCrateSupplies;
    }
}
