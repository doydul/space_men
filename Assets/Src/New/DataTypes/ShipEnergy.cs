namespace DataTypes {

    public class ShipEnergy {

        public const int maxCapacity = 5;

        public int value { get; private set; }
        public bool full => value >= maxCapacity;

        public bool Increase() {
            if (full) {
                return false;
            } else {
                value++;
                return true;
            }
        }

        public void Drain() {
            value = 0;
        }
    }
}