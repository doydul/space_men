using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MetaSquad {
    
    [SerializeField] List<MetaSoldier> metaSoldiers = new();
    public int credits;

    public IEnumerable<MetaSoldier> GetMetaSoldiers() {
        return metaSoldiers;
    }

    public MetaSoldier GetMetaSoldier(string id) {
        return metaSoldiers.Find(meta => meta.id == id);
    }

    public void AddMetaSoldier(MetaSoldier metaSoldier) {
        var id = Guid.NewGuid().ToString();
        metaSoldier.id = id;
        metaSoldiers.Add(metaSoldier);
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
                armour = new InventoryItem() {
                    name = "Basic",
                    type = InventoryItem.Type.Armour
                },
                weapon = new InventoryItem() {
                    name = weapons[i],
                    type = InventoryItem.Type.Weapon
                }
            });
        }
        return result;
    }
}