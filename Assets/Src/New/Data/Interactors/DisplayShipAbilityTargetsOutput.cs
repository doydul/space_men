namespace Data {
    
    public struct DisplayShipAbilityTargetsOutput {

        public ShipAction[] possibleActions;
        public SoldierDisplayInfo[] possibleTargetMetaSoldiers;
    }

    public struct ShipAction {

        public ShipAbilityType type;
        public Position target;
    }
}
