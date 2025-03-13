[System.Serializable]
public class InventoryItem {

    public enum Type {
        Weapon,
        Armour
    }

    public string name;
    public Type type;

    public bool isWeapon => type == Type.Weapon;
    public bool isArmour => type == Type.Armour;
    public bool isDefault => isWeapon && name == PlayerSave.current.inventory.defaultWeapon.name || isArmour && name == PlayerSave.current.inventory.defaultArmour.name;

    public InventoryItem Dup() {
        return new InventoryItem() {
            name = name,
            type = type
        };
    }
}
