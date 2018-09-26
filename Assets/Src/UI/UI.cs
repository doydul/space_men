using UnityEngine;

public class UI : MonoBehaviour, IUserInterface {

    public void Select(Soldier soldier) {
        soldier.GetComponent<SoldierUIController>().Select();
    }

    public void Deselect(Soldier soldier) {
        soldier.GetComponent<SoldierUIController>().Deselect();
    }

    public void ShowMovementIndicators(Soldier soldier) {
        soldier.GetComponent<SoldierUIController>().ShowMovementIndicators();
    }

    public void ShowAmmoIndicators(Soldier soldier) {
        soldier.GetComponent<SoldierUIController>().ShowAmmoIndicators();
    }
}
