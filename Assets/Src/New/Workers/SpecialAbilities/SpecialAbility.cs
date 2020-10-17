using Data;

namespace Workers {
    public abstract class SpecialAbility {

        public abstract bool usable { get; }
        public abstract SpecialActionType type { get; }
        public virtual Position[] possibleTargetSquares { get {
            return new Position[0];
        } }
        public virtual bool executeImmediately => false;
        
        public abstract SpecialAbilityOutput Execute();
    }
}