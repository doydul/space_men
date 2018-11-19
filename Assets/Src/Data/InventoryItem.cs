[System.Serializable]
public class InventoryItem {

    public string name;
    public bool isWeapon;

    public InventoryItem Dup() {
        return new InventoryItem() {
            name = name,
            isWeapon = isWeapon
        };
    }
}
