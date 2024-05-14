public abstract class Objective {
    public bool required;
    public bool done { get; protected set; }

    public abstract void Init(Map.Room room);
    public abstract bool complete { get; }
}