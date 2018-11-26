using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WeaponSelectMode : SelectionMode {

    private SoldierData weaponOwner;
    private List<SelectableItem> selectableWeapons;
    private WeaponItem selectedWeapon;

    private bool canUseHeavyWeapons { get { return Armour.Get(weaponOwner.armour).isMedium; } }

    public WeaponSelectMode(SoldierData weaponOwner) {
        this.weaponOwner = weaponOwner;

        var weapons = Squad.items.Where((item) => {
            if (item.isWeapon) {
                return canUseHeavyWeapons || !Weapon.Get(item.name).isHeavy;
            } else {
                return false;
            }
        }).Select(item => item.name).ToList();

        selectableWeapons = new List<SelectableItem>();
        selectableWeapons.Add(new WeaponItem() { weaponOwner = weaponOwner });
        foreach (var weapon in weapons) {
            selectableWeapons.Add(new WeaponItem() { weapon = weapon });
        }
        foreach (var soldier in Squad.reserveSoldiers) {
            selectableWeapons.Add(new WeaponItem() { weaponOwner = soldier });
        }
    }

    public List<SelectableItem> selectableItems { get { return selectableWeapons; } }

    public void Select(SelectableItem item) {
        selectedWeapon = (WeaponItem)item;
    }

    public void Finalise() {
        if (selectedWeapon.GetWeaponString() != weaponOwner.weapon) {
            var oldWeapon = weaponOwner.weapon;
            if (selectedWeapon.alreadyOwned) {
                weaponOwner.weapon = selectedWeapon.weaponOwner.weapon;
                selectedWeapon.weaponOwner.weapon = oldWeapon;
            } else {
                weaponOwner.weapon = selectedWeapon.weapon;
                Squad.items.Remove(Squad.items.Single(item => item.name == selectedWeapon.weapon));
                Squad.items.Add(new InventoryItem() { name = oldWeapon, isWeapon = true });
            }
        }
        SoldierViewController.OpenMenu(weaponOwner);
    }

    private class WeaponItem : SelectableItem {

        public string weapon;
        public SoldierData weaponOwner;

        public bool alreadyOwned { get { return weaponOwner != null; } }

        public string GetWeaponString() {
            if (weapon != null) {
                return weapon;
            } else {
                return weaponOwner.weapon;
            }
        }

        public string leftText { get {
            return GetWeaponString();
        } }

        public string rightText { get {
            return alreadyOwned ? "owned" : "";
        } }

        public Sprite sprite { get {
            return Resources.Load<Sprite>("Textures/Weapons/Weapon1");
        } }
    }
}
