namespace Data { 
  
    public abstract class Actor {

        public Position position { get; set; }
        public Health health { get; set; }
        public long uniqueId { get; set; }
        public Direction facing { get; set; }
        
        public virtual bool exists { get { return true; } }
        public virtual bool isSoldier { get { return false; } }
        public virtual bool isAlien { get { return false; } }
        public virtual bool isCrate { get { return false; } }

        public void SetUniqueId(long id) {
            if (uniqueId != 0) throw new System.Exception("Unique ID can only be set once");
            uniqueId = id;
        }
    }
}
