namespace Data {
    
    public struct MoveSoldierOutput {
        
        public long soldierIndex;
        public Position newPosition;
        public Position[] traversedCells;
        public Direction newFacing;
        public MovementType movementType;
        public Fog[] newFogs;
    }
}
