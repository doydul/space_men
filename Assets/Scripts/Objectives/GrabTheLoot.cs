using UnityEngine;

public class GrabTheLoot : Objective {
    
    public override string description => "find equipment";
    public override bool complete => chest.isOpen;
    public override Vector2 targetLocation => location;
    Vector2 location;
    
    Chest chest;

    public override void Init(Objectives objectives) {
        var room = objectives.GetNextBestRoom();
        roomId = room.id;
        chest = LootGenerator.instance.InstantiateLootChest(LootGenerator.instance.MakeLoot(PlayerSave.current.difficulty), room.centre);
        chest.onOpen = () => AudioManager.ObjectiveComplete();
        location = chest.gridLocation;
        objectives.AddObjective(room, this);
    }
}