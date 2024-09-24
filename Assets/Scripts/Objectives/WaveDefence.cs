using UnityEngine;

public class WaveDefence : Objective {
    
    public override string description => "prepare defence";
    public override bool complete => completed;
    public override Vector2 targetLocation => location;
    public override RoomTemplate[] specialRooms => new RoomTemplate[] { Resources.Load<RoomTemplate>("SpecialRooms/ObjectiveRooms/WaveDefenceRoom") };
    Vector2 location;
    bool completed;
    
    public override void Init(Objectives objectives) {
        // var room = objectives.GetNextBestRoom();
        // chest = LootGenerator.instance.InstantiateLootChest(LootGenerator.instance.MakeLoot(PlayerSave.current.difficulty), room.centre);
        // location = chest.gridLocation;
        // objectives.AddObjective(room, this);
    }
}