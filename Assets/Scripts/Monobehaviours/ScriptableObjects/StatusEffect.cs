using UnityEngine;

public abstract class StatusEffect : ScriptableObject {
    
    public enum TickPoint {
        None,
        StartOfTurn,
        EndOfTurn
    }
    
    public string inGameName;
    public string description;
    public Sprite sprite;
    public Color tint;
    public TickPoint tickPoint;
    
    protected Actor actor;
    
    public void Apply(Actor target) {
        var clone = Instantiate(this);
        clone.name = name;
        target.AddStatus(clone);
        clone.actor = target;
        clone.OnApply();
    }
    
    public void StartOfTurn() {
        if (tickPoint == TickPoint.StartOfTurn) OnTick();
    }
    public void EndOfTurn() {
        if (tickPoint == TickPoint.EndOfTurn) OnTick();
    }
    public void Remove() {
        actor.RemoveStatus(this);
        OnRemove();
    }
    
    protected virtual void OnApply() {}
    protected virtual void OnTick() {}
    protected virtual void OnRemove() {}
}