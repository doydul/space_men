using UnityEngine;
using System;
using System.Linq;
using System.Collections;

public class ActivateTerminal : Objective {
    
    public const int idealDistance = 20;
    
    public override string description => "activate terminal";
    public override bool complete => terminalActivated;
    public override Vector2 targetLocation => location;
    Vector2 location;
    ActivateTerminal other;
    Terminal terminal;
    bool terminalActivated;
    
    public override void Init(Objectives objectives) {
        var room1 = objectives.GetNextBestRoom();
        PrivInit(room1, objectives);
        other = new ActivateTerminal { required = required };
        var room2 = objectives.GetUnoccupiedRoomWithIdealDistance(room1, idealDistance);
        other.PrivInit(room2, objectives);
        other.other = this;
    }
    
    void PrivInit(Map.Room room, Objectives objectives) {
        location = room.centre;
        InstantiateTerminal(location, PerformTerminalInteract);
        objectives.AddObjective(room, this);
        GameEvents.On(this, "alien_turn_start", CheckOther);
    }
    
    void CheckOther() {
        if (!other.terminalActivated) {
            terminalActivated = false;
            terminal.interactEnabled = true;
        }
    }
    
    IEnumerator PerformTerminalInteract() {
        terminalActivated = true;
        terminal.interactEnabled = false;
        ui?.SetCheckboxState();
        yield return NotificationPopup.PerformShow("", "terminal activated", new BtnData("ok", () => {}));
    }
    
    Terminal InstantiateTerminal(Vector2 gridLocation, Func<IEnumerator> callback) {
        var trans = UnityEngine.Object.Instantiate(Resources.Load<Transform>("Prefabs/Terminal")) as Transform;
        var terminal = trans.GetComponent<Terminal>();
        this.terminal = terminal;
        terminal.action = callback;
        terminal.interactEnabled = true;
        
        var tile = Map.instance.GetTileAt(gridLocation);
        tile.SetActor(trans, true);
        return terminal;
    }
    
    ~ActivateTerminal() => GameEvents.RemoveListener(this, "alien_turn_start");
}