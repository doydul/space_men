namespace Data { 
  
    public class AlienActor : Actor {

        public override bool isAlien { get { return true; } }

        public int movement { get; set; }
        public string type { get; set; }
        public int movesRemaining { get; set; }
    }
}
