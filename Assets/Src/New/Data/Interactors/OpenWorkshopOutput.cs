namespace Data {
    
    public struct OpenWorkshopOutput {
        
        public WorkshopState state;
    }

    public struct WorkshopState {

        public WorkshopItem[] items;
        public WorkshopItem[] blueprints;
        public int credits;
    }

    public struct WorkshopItem {

        public long itemId;
        public string itemName;
        public WorkshopItemType type;
    }

    public enum WorkshopItemType {
        Weapon,
        Armour
    }
}
