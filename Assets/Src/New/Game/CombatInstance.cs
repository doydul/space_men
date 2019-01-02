public class CombatInstance {

    public enum Status {
        Hit,
        Missed,
        Deflected
    }

    public Actor target { get; private set; }
    public Status status { get; private set; }
    public int damageInflicted { get; private set; }

    public CombatInstance(Actor target, Status status, int damageInflicted = 0) {
        this.target = target;
        this.status = status;
        this.damageInflicted = damageInflicted;
    }
}
