using UnityEngine;
using UnityEngine.UI;

using Data;

public class SoldierArmouryPanel : MonoBehaviour {

    public ArmouryController controller;
    public GameObject soldierPicture;
    public GameObject invButton;

    SoldierDisplayInfo soldierInfo;

    public void SetSoldierInfo(SoldierDisplayInfo soldierInfo) {
        this.soldierInfo = soldierInfo;
        if (this.soldierInfo.soldierId == null) {
            soldierPicture.SetActive(false);
            invButton.SetActive(false);
        } else {
            soldierPicture.SetActive(true);
            invButton.SetActive(true);
        }
    }

    public void SelectSoldier() {
        controller.GoToSelectSoldierScreen(soldierInfo.soldierId);
    }

    public void OpenInventory() {
        controller.GoToInventoryScreen(soldierInfo.soldierId);
    }
}
