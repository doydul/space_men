using UnityEngine;
using UnityEngine.UI;

public class SoldierPanelController : MonoBehaviour {

    public ArmouryMenuController menuController;
    public GameObject soldierPicture;
    public GameObject invButton;

    private int _soldierIndex;
    public int soldierIndex {
        set {
            _soldierIndex = value;
        }
    }
    public SoldierData soldier {
        get {
            return Squad.activeSoldiers[_soldierIndex];

            // _soldier = value;
            // if (_soldier == null) {
            //     soldierPicture.SetActive(false);
            //     invButton.SetActive(false);
            // }
        }
    }

    public void SelectSoldier() {
        Debug.Log(_soldierIndex);
        menuController.ClickSoldier(_soldierIndex);
    }

    public void OpenInventory() {

    }
}
