namespace Data {
    
    public struct OpenShipAbilitiesOutput {
        
        public ShipAbilityInfo[] abilities;
    }

    public struct ShipAbilityInfo {

        public ShipAbilityType type;
        public bool usable;
        public ShipAbilityCondition reasonForBeingUnusable;
    }

    public enum ShipAbilityType {
        TeleportSoldierIn,
        TeleportAmmoIn
    }

    public enum ShipAbilityCondition {
        None,
        NeedsShootingPhase,
        NeedsMovementPhase,
        NeedsEnergy,
        NeedsResources
    }
}
