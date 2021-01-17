namespace Data { 
  
    public class FlameActor : Actor {

        public override bool isFlame { get { return true; } }
        public int damage { get; set; }
        public string weaponName { get; set; }
    }
}
