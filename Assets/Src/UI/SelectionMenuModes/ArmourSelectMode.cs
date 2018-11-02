using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ArmourSelectMode : SelectionMode {

    private SoldierData armourOwner;
    private List<SelectableItem> selectableArmours;
    private string selectedArmour;

    public ArmourSelectMode(SoldierData armourOwner) {
        this.armourOwner = armourOwner;

        var armours = Squad.items.Where(item => !item.isWeapon).Select(item => item.name).ToList();
        armours.Add(armourOwner.armour);

        selectableArmours = new List<SelectableItem>();
        foreach (var armour in armours) {
            selectableArmours.Add(new ArmourItem(armour));
        }
    }

    public List<SelectableItem> selectableItems { get { return selectableArmours; } }

    public void Select(SelectableItem item) {
        selectedArmour = ((ArmourItem)item).armour;
    }

    public void Finalise() {
        if (selectedArmour != armourOwner.armour) {
            var oldArmour = armourOwner.armour;
            armourOwner.armour = selectedArmour;
            Squad.items.Remove(Squad.items.Single(item => item.name == selectedArmour));
            Squad.items.Add(new InventoryItem() { name = oldArmour, isWeapon = false });
        }
        SoldierViewController.OpenMenu(armourOwner);
    }

    private class ArmourItem : SelectableItem {

        public string armour;

        public ArmourItem(string armour) {
            this.armour = armour;
        }

        public string leftText { get {
            return armour;
        } }

        public string rightText { get {
            return "";
        } }

        public Sprite sprite { get {
            return Resources.Load<Sprite>("Textures/Armour/Armour1");
        } }
    }
}
