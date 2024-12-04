using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MetaSquad {
    
    [SerializeField] List<MetaSoldier> metaSoldiers = new();

    public IEnumerable<MetaSoldier> GetMetaSoldiers() => metaSoldiers;
    public int Count => metaSoldiers.Count;

    public MetaSoldier GetMetaSoldier(string id) {
        return metaSoldiers.Find(meta => meta.id == id);
    }

    public void AddMetaSoldier(MetaSoldier metaSoldier) {
        var id = System.Guid.NewGuid().ToString();
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
    
    public static MetaSoldier GenerateDefaultSoldier() {
        return new MetaSoldier() {
            name = "John Doe",
            armour = new InventoryItem() {
                name = "Flak Vest",
                type = InventoryItem.Type.Armour
            },
            weapon = new InventoryItem() {
                name = "SIKR-5", 
                type = InventoryItem.Type.Weapon
            },
            tint = new Color(Random.value * 0.5f + 0.5f, Random.value * 0.5f + 0.5f, Random.value * 0.5f + 0.5f)
        };
    }

    public static MetaSquad GenerateDefault() {
        var result = new MetaSquad();
        for (int i = 0; i < 3; i++) {
            result.AddMetaSoldier(GenerateDefaultSoldier());
        }
        return result;
    }
}