using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class LootGenerator : MonoBehaviour {
    
    public static LootGenerator instance;
    void Awake() => instance = this;

    private Weapon[] allWeapons => Resources.LoadAll<Weapon>("Weapons/");
    private Armour[] allArmour => Resources.LoadAll<Armour>("Armour/");

    public Loot MakeLoot(float techLevel) {
        int lootLevel = techLevel < 1 ? 1 : (int)techLevel;
        float upgradeChance = techLevel - lootLevel;
        if (Random.value < upgradeChance) lootLevel += 1;

        var result = new Loot();
        if (Random.value < 0.66f) {
            result.item = new InventoryItem {
                type = InventoryItem.Type.Weapon,
                name = allWeapons.Where(wep => wep.techLevel == lootLevel).WeightedSelect().name
            };
        } else {
            result.item = new InventoryItem {
                type = InventoryItem.Type.Armour,
                name = allArmour.Where(arm => arm.techLevel == lootLevel).WeightedSelect().name
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