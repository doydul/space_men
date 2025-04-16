using UnityEngine;
using System.Linq;

public class LootGenerator : MonoBehaviour {
    
    const float megaChestChance = 1f;
    
    public static LootGenerator instance;
    void Awake() => instance = this;

    private Weapon[] allWeapons => Resources.LoadAll<Weapon>("Weapons/");
    private Armour[] allArmour => Resources.LoadAll<Armour>("Armour/");

    public Loot MakeLoot(float techLevel) {
        var result = new Loot(RandomItem(techLevel));
        if (Random.value < megaChestChance) {
            result.AddItem(RandomItem(techLevel));
        }
        return result;
    }
    
    public Loot MakeCommonLoot(float techLevel) {
        if (Random.value < megaChestChance) {
            return new Loot(RandomItem(techLevel));
        } else {
            return new Loot(Mathf.Max(MathUtil.GuassianInt((techLevel + 1) * 50, 20), 10));
        }
    }

    public Chest InstantiateLootChest(Loot loot, Vector2 gridLocation, bool hidden = false) {
        var trans = Instantiate(Resources.Load<Transform>("Prefabs/Chest")) as Transform;
        var chest = trans.GetComponent<Chest>();
        chest.contents = loot;
        
        var tile = Map.instance.GetTileAt(gridLocation);
        tile.SetActor(trans, true);
        if (hidden) tile.HideBackground();
        return chest;
    }
    
    InventoryItem RandomItem(float techLevel) {
        int lootLevel = techLevel < 1 ? 1 : (int)techLevel;
        float upgradeChance = techLevel - lootLevel;
        if (Random.value < upgradeChance) lootLevel += 1;
        if (Random.value < 0.66f) {
            return new InventoryItem {
                type = InventoryItem.Type.Weapon,
                name = allWeapons.Where(wep => wep.techLevel == lootLevel).WeightedSelect().name
            };
        } else {
            return new InventoryItem {
                type = InventoryItem.Type.Armour,
                name = allArmour.Where(arm => arm.techLevel == lootLevel).WeightedSelect().name
            };
        }
    }
}