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

    public InventoryItem Dup() {
        return new InventoryItem() {
            name = name,
            type = type
        };
    }
}
