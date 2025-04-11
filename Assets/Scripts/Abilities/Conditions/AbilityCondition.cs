public abstract class AbilityCondition {
    
    protected Ability ability;
    protected Soldier owner => ability.owner;
    
    public void Attach(Ability ability) {
        this.ability = ability;
    }
    
    public virtual bool met => true;
    public virtual string explanation => "";
}