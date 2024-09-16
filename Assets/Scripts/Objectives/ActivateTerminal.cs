using UnityEngine;
using System.Linq;

public class ActivateTerminal : Objective {
    
    public const int idealDistance = 20;
    
    public override string description => "activate terminal";
    public override bool complete => false;
    public override Vector2 targetLocation => location;
    Vector2 location;
    ActivateTerminal other;
    
    public override void Init(Objectives objectives) {
        var room1 = objectives.GetNextBestRoom();
        PrivInit(room1, objectives);
        other = new ActivateTerminal { required = required };
        var room2 = objectives.GetUnoccupiedRoomWithIdealDistance(room1, idealDistance);
        other.PrivInit(room2, objectives);
    }
    
    void PrivInit(Map.Room room, Objectives objectives) {
        location = room.centre;
        InstantiateTerminal(location);
        objectives.AddObjective(room, this);
    }
    
    Terminal InstantiateTerminal(Vector2 gridLocation) {
        var trans = Object.Instantiate(Resources.Load<Transform>("Prefabs/Terminal")) as Transform;
        var terminal = trans.GetComponent<Terminal>();
        
        var tile = Map.instance.GetTileAt(gridLocation);
        tile.SetActor(trans, true);
        return terminal;
    }
}