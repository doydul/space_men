using UnityEngine;

public class SpriteSelector {

    private InventoryItem item;

    public SpriteSelector(InventoryItem item) {
        this.item = item;
    }

    public Sprite Select() {
        if (item == null) return null;
        if (item.isWeapon) {
            return Resources.Load<Sprite>("Textures/Weapons/Weapon1");
        } else {
            return Resources.Load<Sprite>("Textures/Armour/Armour1");
        }
    }
}
