namespace Workers {

    public class Unlockable {

        public Unlockable parent { get; private set; }
        public UnlockableType type { get; private set; }

        public Unlockable(UnlockableType type, Unlockable parent) {
            this.type = type;
            this.parent = parent;
        }
    }
}