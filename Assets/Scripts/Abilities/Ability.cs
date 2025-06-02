using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public abstract class Ability : ScriptableObject {
    
    [TextArea] public string description;
    public Sprite sprite;
    public Color spriteColor = new Color(0.6868184f, 0.764151f, 0.5940192f);
    public string hotKey;

    protected AbilityCondition[] conditions;
    public string userFacingName => StringUtils.UserFacingName(name);
    [HideInInspector] public  Soldier owner;

    public virtual void Attach(Soldier owner) {
        var clone = Instantiate(this);
        clone.name = name;
        owner.abilities.Add(clone);
        clone.owner = owner;
        clone.Setup();
        clone.conditions = clone.Conditions().ToArray();
        foreach (var condition in clone.conditions) condition.Attach(clone);
    }
    
    public bool CanUse() => !conditions.Any(condition => !condition.met);
    public string CantUseExplanation() => String.Join(", ", conditions.Where(condition => !condition.met).Select(condition => condition.explanation));
    
    public virtual void Setup() {}
    public virtual void Teardown() {}
    public virtual IEnumerable<AbilityCondition> Conditions() => new AbilityCondition[0];
    public virtual void Use() {}
    public virtual void Display(AbilityIcon icon) {}
}