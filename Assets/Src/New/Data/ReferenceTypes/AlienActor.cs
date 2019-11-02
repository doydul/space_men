namespace Data { 
  
    public class AlienActor : Actor {

        public override bool isAlien { get { return true; } }

        public int movement { get; set; }
        public AlienType type { get; set; }
    }
}
