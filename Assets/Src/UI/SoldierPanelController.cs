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
        }
    }

    void Awake() {
        if (Squad.activeSoldiers.Count <= _soldierIndex || Squad.activeSoldiers[_soldierIndex] == null) {
            soldierPicture.SetActive(false);
            invButton.SetActive(false);
        }
    }

    public void SelectSoldier() {
        menuController.ClickSoldier(_soldierIndex);
    }

    public void OpenInventory() {
        menuController.ViewSoldier(Squad.activeSoldiers[_soldierIndex]);
    }
}
