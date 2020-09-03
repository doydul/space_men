using UnityEngine;
using UnityEngine.UI;

using Data;

public class SoldierArmouryPanel : MonoBehaviour {

    public ArmouryController controller;
    public SoldierIcon soldierIcon;
    public GameObject invButton;

    SoldierDisplayInfo soldierInfo;
    int index;

    public void SetSoldierInfo(int index, SoldierDisplayInfo soldierInfo) {
        this.index = index;
        this.soldierInfo = soldierInfo;
        soldierIcon.SetSoldierInfo(soldierInfo);
        if (this.soldierInfo == null) {
            invButton.SetActive(false);
        } else {
            invButton.SetActive(true);
        }
    }

    public void SelectSoldier() {
        controller.GoToSelectSoldierScreen(index);
    }

    public void OpenInventory() {
        controller.GoToInventoryScreen(soldierInfo.soldierId);
    }
}
