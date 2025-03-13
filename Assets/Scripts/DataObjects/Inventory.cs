using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class Inventory {
    
    public InventoryItem defaultWeapon;
    public InventoryItem defaultArmour;
    List<InventoryItem> _items = new();
    public List<InventoryItem> items => ItemsWithDefaults();
    public List<InventoryItem> blueprints = new();
    
    public bool ContainsBlueprint(InventoryItem item) {
        return blueprints.FirstOrDefault(it => it.name == item.name) != null;
    }
    
    public void AddBlueprint(InventoryItem item) {
        if (ContainsBlueprint(item)) return;
        blueprints.Add(item);
    }

    public void AddItem(InventoryItem item) {
        if (!(item.isWeapon && item.name == defaultWeapon.name || item.isArmour && item.name == defaultArmour.name)) {
            _items.Add(item);
        }
    }
    
    public void RemoveItem(InventoryItem item) {
        _items.Remove(item);
    }

    public void Equip(MetaSoldier soldier, InventoryItem item) {
        RemoveItem(item);
        if (item.type == InventoryItem.Type.Weapon) {
            AddItem(soldier.weapon);
            soldier.weapon = item;
        } else {
            AddItem(soldier.armour);
            soldier.armour = item;
        }
    }
    
    List<InventoryItem> ItemsWithDefaults() {
        var result = new List<InventoryItem>(_items);
        result.Insert(0, defaultArmour);
        result.Insert(0, defaultWeapon);
        return result;
    }
}