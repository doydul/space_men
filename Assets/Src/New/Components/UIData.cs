using UnityEngine;

using Data;
using System;

public class UIData : MonoBehaviour {

    public static UIData instance { get; private set; }

    public ActorAction[] actorActions { get; set; }
    public Data.GamePhase gamePhase { get; set; }
    public Tile selectedTile { get; set; }

    void Awake() {
        instance = this;
        actorActions = new ActorAction[0];
    }

    public bool ActionFor(Tile tile, out ActorAction actorAction) {
        actorAction = default(ActorAction);
        var tilePosition = new Position((int)tile.gridLocation.x, (int)tile.gridLocation.y);
        foreach (var action in actorActions) {
            if (action.target == tilePosition) {
                actorAction = action;
                return true;
            }
        }
        return false;
    }

    public void ClearSelection() {
        actorActions = new ActorAction[0];
        selectedTile = null;
    }
}