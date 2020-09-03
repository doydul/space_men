using UnityEngine;
using UnityEngine.UI;

using Data;

public class SoldierArmouryPanel : MonoBehaviour {

    public ArmouryController controller;
    public GameObject soldierPicture;
    public GameObject invButton;

    SoldierDisplayInfo soldierInfo;
    int index;

    public void SetSoldierInfo(int index, SoldierDisplayInfo soldierInfo) {
        this.index = index;
        this.soldierInfo = soldierInfo;
        if (this.soldierInfo == null) {
            soldierPicture.SetActive(false);
            invButton.SetActive(false);
        } else {
            soldierPicture.SetActive(true);
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
