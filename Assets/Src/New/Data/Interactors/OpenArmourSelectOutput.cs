namespace Data {
    
    public struct OpenArmourSelectOutput {
        
        public long soldierId;
        public ArmourInfo[] inventoryArmour;
        public ArmourInfo currentArmour;

        public struct ArmourInfo {
            public long itemId;
            public string name;
            public ArmourStats armourStats;
        }
    }
}
