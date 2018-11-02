using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ArmourSelectMode : SelectionMode {

    private SoldierData armourOwner;
    private List<SelectableItem> selectableArmours;
    private ArmourItem selectedArmour;

    public ArmourSelectMode(SoldierData armourOwner) {
        this.armourOwner = armourOwner;

        var armours = Squad.items.Where(item => item.isWeapon).Select(item => item.name).ToList();

        selectableArmours = new List<SelectableItem>();
        selectableArmours.Add(new ArmourItem() { armourOwner = armourOwner });
        foreach (var armour in armours) {
            selectableArmours.Add(new ArmourItem() { armour = armour });
        }
        foreach (var soldier in Squad.reserveSoldiers) {
            selectableArmours.Add(new ArmourItem() { armourOwner = soldier });
        }
    }

    public List<SelectableItem> selectableItems { get { return selectableArmours; } }

    public void Select(SelectableItem item) {
        selectedArmour = (ArmourItem)item;
    }

    public void Finalise() {
        if (selectedArmour.GetArmourString() != armourOwner.armour) {
            var oldArmour = armourOwner.armour;
            if (selectedArmour.alreadyOwned) {
                armourOwner.armour = selectedArmour.armourOwner.armour;
                selectedArmour.armourOwner.armour = oldArmour;
            } else {
                armourOwner.armour = selectedArmour.armour;
                Squad.items.Remove(Squad.items.Single(item => item.name == selectedArmour.armour));
                Squad.items.Add(new InventoryItem() { name = oldArmour, isWeapon = false });
            }
        }
        SoldierViewController.OpenMenu(armourOwner);
    }

    private class ArmourItem : SelectableItem {

        public string armour;
        public SoldierData armourOwner;

        public bool alreadyOwned { get { return armourOwner != null; } }

        public string GetArmourString() {
            if (armour != null) {
                return armour;
            } else {
                return armourOwner.armour;
            }
        }

        public string leftText { get {
            return GetArmourString();
        } }

        public string rightText { get {
            return alreadyOwned ? "owned" : "";
        } }

        public Sprite sprite { get {
            return Resources.Load<Sprite>("Textures/Armour/Armour1");
        } }
    }
}
