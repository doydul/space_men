using Data;

namespace Workers {

    public abstract class ShipAbility {

        public abstract bool usable { get; }
        public abstract ShipAbilityType type { get; }
        public virtual Position[] possibleTargetSquares { get {
            return new Position[0];
        } }
        public virtual MetaSoldier[] possibleTargetMetaSoldiers { get {
            return new MetaSoldier[0];
        } }

        public abstract ShipAbilityOutput Execute();
    }
}