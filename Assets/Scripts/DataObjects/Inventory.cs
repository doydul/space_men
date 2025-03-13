using System.Collections.Generic;

[System.Serializable]
public class Inventory {
    
    public InventoryItem defaultWeapon;
    public InventoryItem defaultArmour;
    List<InventoryItem> _items = new();
    public List<InventoryItem> items => ItemsWithDefaults();
    public List<InventoryItem> blueprints = new();

    public void AddItem(InventoryItem item) {
        if (!(item.isWeapon && item.name == defaultWeapon.name || item.isArmour && item.name == defaultArmour.name)) {
            _items.Add(item);
        }
    }

    public void Equip(MetaSoldier soldier, InventoryItem item) {
        _items.Remove(item);
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