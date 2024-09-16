using System.Collections;

public class Terminal : Actor {
    
    public override bool interactable => true;
    
    public override IEnumerator PerformUse(Soldier user) {
        yield return null;
    }
}