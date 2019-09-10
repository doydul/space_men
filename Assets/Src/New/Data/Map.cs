namespace Data {

    public struct Cell {

        public bool isWall;
        public bool isFoggy;
        public Position position;
        public ActorType actorType;
        public int soldierIndex;
        public int alienIndex;
        public bool isSpawnPoint;
        public bool isAlienSpawnPoint;
    }

    public enum ActorType {
        None,
        Soldier,
        Alien
    }
}
