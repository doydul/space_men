using UnityEngine;
using UnityEngine.UI;

public class SoldierSelectPanelController : MonoBehaviour {

    public SoldierSelectMenuController menuController;
    public GameObject soldierPicture;

    private SoldierData _soldierData;
    public SoldierData soldierData {
        set {
            _soldierData = value;
        }
    }

    void Start() {
        if (_soldierData == null) soldierPicture.SetActive(false);

    }

    public void SelectSoldier() {
        menuController.SelectReplacementSoldier(_soldierData);
    }
}
