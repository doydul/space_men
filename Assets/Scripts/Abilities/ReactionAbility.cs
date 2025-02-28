using System.Collections;

public abstract class ReactionAbility : Ability {
    
    public int reactionPriority;
    
    public abstract bool TriggersReaction(Tile tile, Actor actor);
    public abstract IEnumerator PerformReaction(Tile tile);
}