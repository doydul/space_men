using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WaveDefence : Objective {
    
    public override string description => "prepare defence";
    public override bool complete => waveCounter <= 0;
    public override Vector2 targetLocation => location;
    public override RoomTemplate[] specialRooms => new RoomTemplate[] { Resources.Load<RoomTemplate>("SpecialRooms/ObjectiveRooms/WaveDefenceRoom") };
    Vector2 location;
    Terminal terminal;
    bool terminalActivated;
    int waveCounter;
    
    public override void Init(Objectives objectives) {
        var room = Map.instance.rooms[roomId];
        location = room.centre;
        InstantiateTerminal(location, PerformTerminalInteract);
        objectives.AddObjective(room, this);
        GameEvents.On(this, "alien_turn_start", SpawnAliens);
        waveCounter = 3;
    }
    
    void SpawnAliens() {
        if (terminalActivated && waveCounter > 0) {
            var spawnings = new List<HiveMind.Spawning>();
            foreach (var tracker in HiveMind.instance.spawnTrackers) {
                int threat = tracker.startingThreat / 6;
                int groupSize = tracker.profile.AvailableGroupSize(Map.instance.enemyProfiles);
                while (threat > 0) {
                    spawnings.Add(new HiveMind.Spawning { type = tracker.profile.typeName, number = groupSize });
                    threat -= tracker.profile.threat * groupSize;
                }
            }
            HiveMind.instance.Spawn(spawnings);
            waveCounter--;
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
    
    ~WaveDefence() => GameEvents.RemoveListener(this, "alien_turn_start");
}