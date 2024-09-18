using UnityEngine;
using System.Collections;

public abstract class AlienBehaviour : ScriptableObject {

    protected Alien body;

    public virtual void Attach(Alien body) {
        var clone = Instantiate(this);
        clone.name = name;
        body.behaviour = clone;
        clone.body = body;
    }
    
    public abstract IEnumerator PerformTurn();
}