using UnityEngine;

public abstract class Objective {
    public bool required;
    public abstract void Init(Objectives objectives);
    public abstract bool complete { get; }
    public abstract string description { get; }
    public abstract Vector2 targetLocation { get; }
}