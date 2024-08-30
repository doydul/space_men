using UnityEngine;

public class GrabTheLoot : Objective {
    
    public override string description => "find equipment";
    public override bool complete => chest == null;
    public override Vector2 targetLocation => location;
    Vector2 location;
    
    Chest chest;

    public override void Init(Map.Room room) {
        chest = LootGenerator.instance.InstantiateLootChest(LootGenerator.instance.MakeLoot(PlayerSave.current.difficulty), room.centre);
        location = chest.gridLocation;
    }
}