using System.Collections;

public class Door : Actor {
    
    public enum Facing {
        None,
        NorthSouth,
        EastWest
    }
    
    public override bool interactable => true;
    
    public override IEnumerator PerformUse(Soldier user) {
        yield return GameplayOperations.PerformOpenDoor(user, tile);
    }
    
    public void SetFacing(Facing facing) {
        var tmp = transform.localEulerAngles;
        if (facing == Facing.NorthSouth) tmp.z = 90;
        else tmp.z = 0;
        transform.localEulerAngles = tmp;
    }
}