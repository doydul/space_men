using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MetaSquad {
    
    [SerializeField] List<MetaSoldier> metaSoldiers = new();

    public IEnumerable<MetaSoldier> GetMetaSoldiers() => metaSoldiers;
    public int Count => metaSoldiers.Count;

    public MetaSoldier GetMetaSoldier(string id) {
        return metaSoldiers.Find(meta => meta.id == id);
    }

    public void AddMetaSoldier(MetaSoldier metaSoldier) {
        var id = Guid.NewGuid().ToString();
        metaSoldier.id = id;
        metaSoldiers.Add(metaSoldier);
    }
    
    public MetaSoldier RemoveMetaSoldier(MetaSoldier metaSoldier) {
        metaSoldiers.Remove(metaSoldier);
        return metaSoldier;
    }
    public MetaSoldier RemoveMetaSoldier(int index) {
        var metaSoldier = metaSoldiers[index];
        metaSoldiers.RemoveAt(index);
        return metaSoldier;
    }
    
    public void RemoveMetaSoldierById(string id) => RemoveMetaSoldier(GetMetaSoldier(id));

    public static MetaSquad GenerateDefault() {
        var result = new MetaSquad();
        var weapons = new string[] {
            "SIKR-5",
            "SIKR-5",
            "SIKR-5",
        };
        for (int i = 0; i < weapons.Length; i++) {
            result.AddMetaSoldier(new MetaSoldier() {
                name = "John Doe",
                armour = new InventoryItem() {
                    name = "Flak Vest",
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