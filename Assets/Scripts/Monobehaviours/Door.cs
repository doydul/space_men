using System.Collections;

public class Door : Actor {
    
    public override bool interactable => true;
    
    public override IEnumerator PerformUse(Soldier user) {
        yield return GameplayOperations.PerformOpenDoor(user, tile);
    }
}