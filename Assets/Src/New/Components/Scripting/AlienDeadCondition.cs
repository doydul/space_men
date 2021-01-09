using UnityEngine;

public class AlienDeadCondition : ObjectiveCondition {

    public Alien alien;

    public override bool satisfied { get {
        return alien == null || alien.dead;
    } }
}