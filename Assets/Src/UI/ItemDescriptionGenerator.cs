using System;

public class ItemDescriptionGenerator {

    private InventoryItem item;

    public ItemDescriptionGenerator(InventoryItem item) {
        this.item = item;
    }

    public string Generate() {
        if (item == null) return "";
        if (weapon != null) {
            return String.Join(Environment.NewLine, new string[] {
                "Name: " + weapon.name,
                "Accuracy: " + weapon.accuracy,
                "ArmourPen: " + weapon.armourPen,
                "MinDamage: " + weapon.minDamage,
                "Shots when moving: " + weapon.shotsWhenMoving,
                "Shots when still: " + weapon.shotsWhenStill,
                "Ammo Capacity: " + weapon.ammo
            });
        } else if (armour != null) {
            return String.Join(Environment.NewLine, new string[] {
                "Name: " + armour.name,
                "Armour value: " + armour.armourValue,
                "Run distance: " + armour.movement,
                "Sprint distance: " + armour.sprint
            });
        }
        return "";
    }

    // Private

    private Weapon weapon { get {
        if (item.isWeapon) {
            return Weapon.Get(item.name);
        }
        return null;
    } }

    private Armour armour { get {
        if (!item.isWeapon) {
            return Armour.Get(item.name);
        }
        return null;
    } }
}
