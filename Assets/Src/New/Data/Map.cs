namespace Data {
    
    public struct Map {

        public Cell[,] cells;
    }

    public struct Cell {

        public bool isWall;
        public bool isFoggy;
        public Position position;
        public ActorType actorType;
        public int soldierIndex;
    }

    public enum ActorType {
        None,
        Soldier,
        Alien
    }
}
