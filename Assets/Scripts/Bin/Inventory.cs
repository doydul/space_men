using System.Collections.Generic;

[System.Serializable]
public class Inventory {
    
    public List<InventoryItem> items = new();

    public void Equip(MetaSoldier soldier, InventoryItem item) {
        items.Remove(item);
        if (item.type == InventoryItem.Type.Weapon) {
            items.Add(soldier.weapon);
            soldier.weapon = item;
        } else {
            items.Add(soldier.armour);
            soldier.armour = item;
        }
    }
}