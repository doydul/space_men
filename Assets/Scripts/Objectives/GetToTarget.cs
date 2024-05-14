public class GetToTarget : Objective {

    public override bool complete { get {
        foreach (var soldier in Map.instance.GetActors<Soldier>()) {
            UnityEngine.Debug.Log(soldier, soldier);
            bool found = false;
            foreach (var tile in room.tiles) {
                if (tile.GetActor<Soldier>() == soldier) {
                    UnityEngine.Debug.Log($"found soldier at tile {tile.gridLocation}", tile);
                    found = true;
                    break;
                }
            }
            if (!found) return false;
        }
        return true;
    } }

    public bool onceAndDone;

    Map.Room room;

    public override void Init(Map.Room room) {
        this.room = room;
    }
}