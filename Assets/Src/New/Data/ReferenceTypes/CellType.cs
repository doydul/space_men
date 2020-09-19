namespace Data { 
  
    public class CellType {

        public bool isWall { get; set; }
        public bool isFoggy { get; set; }
        public Position position { get; set; }
        public Data.Actor actor { get; set; }
        public Data.Actor backgroundActor { get; set; }
        public bool hasActor { get { return actor.exists; } }
        public bool isSpawnPoint { get; set; }
        public bool isAlienSpawnPoint { get; set; }
        
        public CellType() {
            actor = new NullActor();
            backgroundActor = new NullActor();
        }
        
        public static CellType FromValueType(Cell valueType) {
            var result = new CellType();
            result.isWall = valueType.isWall;
            result.isFoggy = valueType.isFoggy;
            result.position = valueType.position;
            result.isSpawnPoint = valueType.isSpawnPoint;
            result.isAlienSpawnPoint = valueType.isAlienSpawnPoint;
            return result;
        }

        public void ClearActor() {
            actor = new NullActor();
        }

        public void ClearBackgroundActor() {
            backgroundActor = new NullActor();
        }

        public void RemoveActor(long actorId) {
            if (actor.uniqueId == actorId) {
                ClearActor();
            } else if (backgroundActor.uniqueId == actorId) {
                ClearBackgroundActor();
            }
        }
    }
}

