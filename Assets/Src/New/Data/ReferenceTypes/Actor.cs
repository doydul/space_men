namespace Data { 
  
    public abstract class Actor {

        public Position position { get; private set; }
        public Health health { get; private set; }
        public int movement { get; private set; }
    }
}
