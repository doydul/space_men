namespace Data { 
  
    public abstract class Actor {

        public Position position { get; set; }
        public Health health { get; set; }
        public long uniqueId { get; set; }
        public Direction facing { get; set; }
        
        public virtual bool exists => true;
        public virtual bool isSoldier => false;
        public virtual bool isAlien => false;
        public virtual bool isCrate => false;

        public void SetUniqueId(long id) {
            if (uniqueId != 0) throw new System.Exception("Unique ID can only be set once");
            uniqueId = id;
        }
    }
}
