using System.Collections;

public class Chest : Actor {
    public Loot contents;
    
    public override bool interactable => true;
    
    public override IEnumerator PerformUse(Soldier user) {
        yield return GameplayOperations.PerformPickupChest(user, tile);
    }
}