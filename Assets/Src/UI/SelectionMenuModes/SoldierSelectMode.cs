using System.Collections.Generic;
using UnityEngine;

public class SoldierSelectMode : SelectionMode {

    private int squadPosition;
    private List<SelectableItem> selectableSoldiers;
    private SoldierData selectedSoldier;

    public SoldierSelectMode(int squadPosition) {
        this.squadPosition = squadPosition;
        var soldiers = new List<SoldierData>(Squad.reserveSoldiers);
        soldiers.Add(Squad.activeSoldiers[squadPosition]);

        selectableSoldiers = new List<SelectableItem>();
        foreach (var soldier in soldiers) {
            selectableSoldiers.Add(new SoldierItem(soldier));
        }
    }

    public List<SelectableItem> selectableItems { get { return selectableSoldiers; } }

    public void Select(SelectableItem item) {
        selectedSoldier = ((SoldierItem)item).soldierData;
    }

    public void Finalise() {
        if (selectedSoldier != null) Squad.ReplaceSoldier(squadPosition, selectedSoldier);
        ArmouryMenuController.OpenMenu();
    }

    private class SoldierItem : SelectableItem {

        public SoldierData soldierData;

        public SoldierItem(SoldierData soldierData) {
            this.soldierData = soldierData;
        }

        public string leftText { get {
            return "Armour: " + soldierData.armour + "\n\nWeapon: " + soldierData.weapon;
        } }

        public string rightText { get {
            return "Exp: " + soldierData.exp;
        } }

        public Sprite sprite { get {
            return Resources.Load<Sprite>("Textures/Soldiers/Soldier1");
        } }
    }
}
