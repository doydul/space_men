using System;
using System.Collections;

public class Terminal : Actor {
    
    public override bool interactable => interactEnabled;
    public bool interactEnabled { get; set; }
    public Func<IEnumerator> action { get; set; }
    
    public override IEnumerator PerformUse(Soldier user) {
        if (action != null) yield return action();
        else yield break;
    }
}