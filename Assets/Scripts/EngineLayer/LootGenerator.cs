using UnityEngine;
using System.Collections.Generic;

public class LootGenerator : MonoBehaviour {
    
    public static LootGenerator instance;
    void Awake() => instance = this;

    private Weapon[] allWeapons => Resources.LoadAll<Weapon>("Weapons/");
    private Armour[] allArmour => Resources.LoadAll<Armour>("Armour/");

    public List<Loot> Generate() {
        var result = new List<Loot>();
        for (int i = 0; i < 20; i++) { 
            result.Add(MakeLoot());
        }
        return result;
    }

    public Loot MakeLoot() {
        var result = new Loot();
        if (Random.value < 0.5f) {
            result.item = new InventoryItem {
                type = InventoryItem.Type.Weapon,
                name = allWeapons.Sample().name
            };
        } else {
            result.item = new InventoryItem {
                type = InventoryItem.Type.Armour,
                name = allArmour.Sample().name
            };
        }
        return result;
    }

    public void InstantiateLootChest(Loot loot, Vector2 gridLocation) {
        var trans = Instantiate(Resources.Load<Transform>("Prefabs/Chest")) as Transform;
        var chest = trans.GetComponent<Chest>();
        chest.contents = loot;
        
        Map.instance.GetTileAt(gridLocation).SetActor(trans, true);
    }
}