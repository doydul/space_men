[System.Serializable]
public class ItemBlueprint {

    public InventoryItem item;

    public string name { get { return item.name; } }
    public bool isWeapon { get { return item.isWeapon; } }
    public int value { get { return item.value; } }
}
