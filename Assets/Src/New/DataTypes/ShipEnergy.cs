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

        public void Change(int amount) {
            value += amount;
            if (value < 0) value = 0;
            if (value > maxCapacity) value = maxCapacity;
        }

        public void Drain() {
            value = 0;
        }
    }
}