using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MetaSquad {
    
    Dictionary<string, MetaSoldier> metaSoldiers = new();

    public int credits { get; set; }
    public List<InventoryItem> items = new();

    public IEnumerable<MetaSoldier> GetMetaSoldiers() {
        return metaSoldiers.Values;
    }

    public MetaSoldier GetMetaSoldier(string id) {
        return metaSoldiers[id];
    }

    public void AddMetaSoldier(MetaSoldier metaSoldier) {
        var id = Guid.NewGuid().ToString();
        metaSoldier.id = id;
        metaSoldiers.Add(id, metaSoldier);
    }

    public static MetaSquad GenerateDefault() {
        var result = new MetaSquad();
        var weapons = new string[] {
            "Assault Rifle",
            "Laser",
            "Grenade Launcher",
            "Chain Gun"
        };
        for (int i = 0; i < weapons.Length; i++) {
            result.AddMetaSoldier(new MetaSoldier() {
                name = "John Doe",
                armour = new MetaArmour() {
                    id = Guid.NewGuid().ToString(),
                    name = "Basic"
                },
                weapon = new MetaWeapon() {
                    id = Guid.NewGuid().ToString(),
                    name = weapons[i]
                }
            });
        }
        return result;
    }
}