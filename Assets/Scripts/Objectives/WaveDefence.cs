using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class WaveDefence : Objective {
    
    public override string description => "prepare defence";
    public override bool complete => turnCounter <= 0;
    public override Vector2 targetLocation => location;
    public override RoomTemplate[] specialRooms => new RoomTemplate[] { Resources.Load<RoomTemplate>("SpecialRooms/ObjectiveRooms/WaveDefenceRoom") };
    public override int extraTurns => 5;
    
    Vector2 location;
    Terminal terminal;
    bool terminalActivated;
    int turnCounter;
    List<int> threatCounters;
    
    public override void Init(Objectives objectives) {
        var room = Map.instance.rooms[roomId];
        location = room.centre;
        InstantiateTerminal(location, PerformTerminalInteract);
        objectives.AddObjective(room, this);
        GameEvents.On(this, "alien_turn_start", SpawnAliens);
        GameEvents.On(this, "player_turn_start", ShowAlerts);
        turnCounter = 4;
    }
    
    void SpawnAliens() {
        int totalThreatPerWave = 100;
        int threatPerTrackerPerWave = totalThreatPerWave / HiveMind.instance.spawnTrackers.Where(tracker => tracker.profile.data.spawnsDuringWaveDefence).Count();
        
        if (terminalActivated && turnCounter > 0) {
            if (turnCounter % 2 == 0) {
                var spawnings = new List<HiveMind.Spawning>();
                for (int i = 0; i < threatCounters.Count; i++) {
                    var tracker = HiveMind.instance.spawnTrackers[i];
                    if (!tracker.profile.data.spawnsDuringWaveDefence) continue;
                    threatCounters[i] += threatPerTrackerPerWave;
                    int groupSize = tracker.profile.AvailableGroupSize(PlayerSave.current);
                    int totalThreatCost = tracker.profile.threat * groupSize;
                    while (threatCounters[i] >= totalThreatCost) {
                        spawnings.Add(new HiveMind.Spawning { type = tracker.profile.typeName, number = groupSize });
                        threatCounters[i] -= totalThreatCost;
                    }
                }
                HiveMind.instance.SpawnInFog(2, 4, spawnings);
            }
            turnCounter--;
            if (turnCounter <= 0) AudioManager.ObjectiveComplete();
        }
    }
    
    void ShowAlerts() {
        if (terminalActivated && turnCounter > 0 && turnCounter % 2 == 0) {
            Alert.Show("alien wave incoming!");
        }
    }
    
    IEnumerator PerformTerminalInteract() {
        terminalActivated = true;
        terminal.interactEnabled = false;
        ui?.SetCheckboxState();
        
        foreach (var tile in Map.instance.iterator.RadiallyFrom(terminal.gridLocation, 10)) {
            if (tile.HasActor<Door>()) tile.GetBackgroundActor<Door>().Remove();
        }
        
        threatCounters = new();
        foreach (var tracker in HiveMind.instance.spawnTrackers) threatCounters.Add(0);
        
        yield return NotificationPopup.PerformShow("", "terminal activated", new BtnData("ok", () => {}));
        ShowAlerts();
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
    
    ~WaveDefence() {
        GameEvents.RemoveListener(this, "alien_turn_start");
        GameEvents.RemoveListener(this, "player_turn_start");
    }
}