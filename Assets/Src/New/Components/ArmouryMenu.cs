using UnityEngine;
using Data;

public class ArmouryMenu : MonoBehaviour {
    
    public SoldierArmouryPanel[] soldierPanels;

    public void Init(SoldierDisplayInfo[] soldierInfo) {
        for (int i = 0; i < soldierPanels.Length; i++) {
            soldierPanels[i].SetSoldierInfo(i, soldierInfo[i]);
        }
    }
}