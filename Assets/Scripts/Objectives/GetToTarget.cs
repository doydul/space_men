using UnityEngine;

public class GetToTarget : Objective {

    public override bool complete { get {
        foreach (var soldier in Map.instance.GetActors<Soldier>()) {
            bool found = false;
            foreach (var tile in room.tiles) {
                if (tile.GetActor<Soldier>() == soldier) {
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
        foreach (var tile in room.tiles) tile.SetTint(new Color(0.2f, 0.9f, 0.2f));
    }
}