using UnityEngine;

public abstract class Ability : ScriptableObject {

    public string description;

    protected Soldier owner;

    public virtual void Attach(Soldier owner) {
        var clone = Instantiate(this);
        clone.name = name;
        owner.abilities.Add(clone);
        clone.owner = owner;
    }
    
    public virtual bool CanUse() => true;
    public virtual void Use() {}
}