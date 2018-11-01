using UnityEngine;
using UnityEngine.UI;

public class SoldierPanelController : MonoBehaviour {

    public ArmouryMenuController menuController;
    public GameObject soldierPicture;
    public GameObject invButton;

    private SoldierData _soldier;
    public SoldierData soldier {
        set {
            _soldier = value;
            if (_soldier == null) {
                soldierPicture.SetActive(false);
                invButton.SetActive(false);
            }
        }
    }

    public void SelectSoldier() {
        menuController.ClickSoldier(_soldier);
    }

    public void OpenInventory() {

    }
}
