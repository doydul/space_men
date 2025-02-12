using UnityEngine;

public abstract class StatusEffect : ScriptableObject {
    
    public Sprite sprite;
    public Color tint;
    
    protected Actor actor;
    
    public void Apply(Actor target) {
        var clone = Instantiate(this);
        clone.name = name;
        target.AddStatus(clone);
        clone.actor = target;
        clone.OnApply();
    }
    
    public void Tick() => OnTick();
    public void Remove() {
        actor.RemoveStatus(this);
        OnRemove();
    }
    
    protected virtual void OnApply() {}
    protected virtual void OnTick() {}
    protected virtual void OnRemove() {}
}