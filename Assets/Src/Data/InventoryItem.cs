[System.Serializable]
public class InventoryItem {

    public string name;
    public bool isWeapon;
    public int value;

    public InventoryItem Dup() {
        return new InventoryItem() {
            name = name,
            isWeapon = isWeapon,
            value = value
        };
    }
}
