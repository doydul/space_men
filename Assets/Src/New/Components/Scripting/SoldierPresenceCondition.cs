using UnityEngine;

[RequireComponent(typeof(Tile))]
public class SoldierPresenceCondition : ObjectiveCondition {

    Tile tile;

    void Awake() {
        tile = GetComponent<Tile>();
    }

    public override bool satisfied { get {
        return tile.GetActor<Soldier>() != null;
    } }
}