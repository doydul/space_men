using UnityEngine;
using System.Collections.Generic;

public class LootGenerator : MonoBehaviour {
    
    public static LootGenerator instance;
    void Awake() => instance = this;

    private Weapon[] allWeapons => Resources.LoadAll<Weapon>("Weapons/");
    private Armour[] allArmour => Resources.LoadAll<Armour>("Armour/");

    public List<Loot> Generate() {
        var result = new List<Loot>();
        for (int i = 0; i < 2; i++) { 
            result.Add(MakeLoot());
        }
        return result;
    }

    private Loot MakeLoot() {
        var result = new Loot();
        if (Random.value < 0.5f) {
            result.item = new InventoryItem {
                isWeapon = true,
                name = allWeapons.Sample().name
            };
        } else {
            result.item = new InventoryItem {
                isWeapon = false,
                name = allArmour.Sample().name
            };
        }
        return result;
    }
}