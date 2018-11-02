using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WeaponSelectMode : SelectionMode {

    private SoldierData weaponOwner;
    private List<SelectableItem> selectableWeapons;
    private string selectedWeapon;

    public WeaponSelectMode(SoldierData weaponOwner) {
        this.weaponOwner = weaponOwner;

        var weapons = Squad.items.Where(item => item.isWeapon).Select(item => item.name).ToList();
        weapons.Add(weaponOwner.weapon);

        selectableWeapons = new List<SelectableItem>();
        foreach (var weapon in weapons) {
            selectableWeapons.Add(new WeaponItem(weapon));
        }
    }

    public List<SelectableItem> selectableItems { get { return selectableWeapons; } }

    public void Select(SelectableItem item) {
        selectedWeapon = ((WeaponItem)item).weapon;
    }

    public void Finalise() {
        if (selectedWeapon != weaponOwner.weapon) {
            var oldWeapon = weaponOwner.weapon;
            weaponOwner.weapon = selectedWeapon;
            Squad.items.Remove(Squad.items.Single(item => item.name == selectedWeapon));
            Squad.items.Add(new InventoryItem() { name = oldWeapon, isWeapon = true });
        }
        SoldierViewController.OpenMenu(weaponOwner);
    }

    private class WeaponItem : SelectableItem {

        public string weapon;

        public WeaponItem(string weapon) {
            this.weapon = weapon;
        }

        public string leftText { get {
            return weapon;
        } }

        public string rightText { get {
            return "";
        } }

        public Sprite sprite { get {
            return Resources.Load<Sprite>("Textures/Weapons/Weapon1");
        } }
    }
}
