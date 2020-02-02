using UnityEngine;

public class ArmouryMenu : MonoBehaviour {
    
    public SoldierArmouryPanel[] soldierPanels;

    public void Init(ArmouryMenuArgs args) {
        for (int i = 0; i < soldierPanels.Length; i++) {
            soldierPanels[i].SetSoldierInfo(args.soldierInfo[i]);
        }
    }
}