public abstract class Phase {

    public abstract bool finished { get; }

    public virtual void Start() {}

    public virtual void End() {}

    public virtual void Proceed(System.Action callback) {}
}
