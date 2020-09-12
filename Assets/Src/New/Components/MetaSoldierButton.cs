using UnityEngine;
using System;
using Data;

public class MetaSoldierButton : MonoBehaviour {
    
    SoldierDisplayInfo soldierInfo;
    Action<SoldierDisplayInfo> callback;

    public void DisplaySoldier(SoldierDisplayInfo soldierInfo, Action<SoldierDisplayInfo> callback) {
        this.soldierInfo = soldierInfo;
        this.callback = callback;
    }

    public void DoCallback() {
        callback(soldierInfo);
    }
}