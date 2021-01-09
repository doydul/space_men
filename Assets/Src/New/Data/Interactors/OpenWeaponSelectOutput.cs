namespace Data {
    
    public struct OpenWeaponSelectOutput {
        public long soldierId;
        public WeaponInfo[] inventoryWeapons;
        public WeaponInfo currentWeapon;

        public struct WeaponInfo {
            public long itemId;
            public string name;
            public WeaponStats weaponStats;
        }
    }
}
