using UnityEngine;

using Data;
using System;

public class UIData : MonoBehaviour {

    public static UIData instance { get; private set; }

    public ActorAction[] actorActions { get; set; }
    public ShipAction[] shipActions { get; set; }
    public Data.GamePhase gamePhase { get; set; }
    public int threatLevel { get; set; }
    public Tile selectedTile { get; set; }
    public SoldierDisplayInfo selectedMetaSoldier { get; set; }

    public Actor selectedActor { get { return selectedTile != null ? selectedTile.actor.GetComponent<Actor>() : null; } }

    void Awake() {
        instance = this;
        actorActions = new ActorAction[0];
        shipActions = new ShipAction[0];
    }

    public bool ActionFor(Tile tile, out ActorAction actorAction) {
        actorAction = default(ActorAction);
        var tilePosition = new Position((int)tile.gridLocation.x, (int)tile.gridLocation.y);
        foreach (var action in actorActions) {
            if (action.target == tilePosition) {
                actorAction = action;
                return !(action.type == ActorActionType.PossibleMove);
            }
        }
        return false;
    }

    public bool ShipActionFor(Tile tile, out ShipAction shipAction) {
        shipAction = default(ShipAction);
        var tilePosition = new Position((int)tile.gridLocation.x, (int)tile.gridLocation.y);
        foreach (var action in shipActions) {
            if (action.target == tilePosition) {
                shipAction = action;
                return true;
            }
        }
        return false;
    }

    public void ClearSelection() {
        actorActions = new ActorAction[0];
        shipActions = new ShipAction[0];
        selectedTile = null;
        selectedMetaSoldier = null;
    }
}