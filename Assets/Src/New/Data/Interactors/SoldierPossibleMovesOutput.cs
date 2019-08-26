namespace Data {
    
    public struct SoldierPossibleMovesOutput {
        
        public PossibleMoveLocation[] possibleMoveLocations;
    }
    
    public struct PossibleMoveLocation {
        
        public Position position;
        public bool sprint;
    }
}
