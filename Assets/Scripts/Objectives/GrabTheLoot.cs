public class GrabTheLoot : Objective {
    
    public override bool complete => true;

    public override void Init(Map.Room room) {
        LootGenerator.instance.InstantiateLootChest(LootGenerator.instance.MakeLoot(PlayerSave.current.difficulty), room.centre);
    }
}