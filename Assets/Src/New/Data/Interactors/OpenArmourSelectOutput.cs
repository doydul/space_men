namespace Data {
    
    public struct OpenArmourSelectOutput {
        public ArmourInfo[] inventoryArmours;
        public ArmourInfo currentArmour;

        public struct ArmourInfo {
            public long itemId;
            public string name;
        }
    }
}
