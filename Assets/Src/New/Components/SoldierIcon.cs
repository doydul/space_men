using Data;
using UnityEngine;
using UnityEngine.UI;

public class SoldierIcon : MonoBehaviour {
    public Image image;

    public void SetSoldierInfo(SoldierDisplayInfo soldierData) {
        if (soldierData == null) {
            image.enabled = false;
        }
    }
}