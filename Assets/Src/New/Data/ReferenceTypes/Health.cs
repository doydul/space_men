namespace Data { 
  
    public class Health {
        
        public int current { get; private set; }
        public int max { get; private set; }
        public bool dead { get { return current <= 0; } }

        public Health(int max) {
            this.max = max;
            current = max;
        }
        
        public void Damage(int amount) {
            current -= amount;
        }
    }
}
