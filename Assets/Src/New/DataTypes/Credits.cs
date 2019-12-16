namespace DataTypes {

    public class Credits {

        public int value { get; private set; }

        public Credits(int initialValue) {
            value = initialValue;
        }

        public void Add(int amount) {
            value += amount;
        }

        public void Deduct(int amount) {
            value -= amount;
        }
    }
}