using System;
using Data;
using UnityEngine;

public class MetaSoldierSelectPanel : MonoBehaviour {

    public Transform metaSoldierButtonPrefab;
    public Transform buttonLayout;

    Action<SoldierDisplayInfo> callback;

    void Start() {
        Close();
    }

    public void Close() {
        gameObject.SetActive(false);
    }
    
    public void Open(SoldierDisplayInfo[] soldierDisplayInfo, Action<SoldierDisplayInfo> callback) {
        gameObject.SetActive(true);
        this.callback = callback;
        foreach (Transform button in buttonLayout) {
            Destroy(button.gameObject);
        }
        foreach (var soldier in soldierDisplayInfo) {
            InstantiateSoldierButton(soldier);
        }
    }

    void InstantiateSoldierButton(SoldierDisplayInfo soldier) {
        var button = Instantiate(metaSoldierButtonPrefab) as Transform;
        var script = button.GetComponent<MetaSoldierButton>();
        script.DisplaySoldier(soldier, HandleSoldierClick);
        button.SetParent(buttonLayout);
    }

    void HandleSoldierClick(SoldierDisplayInfo soldierInfo) {
        Close();
        callback(soldierInfo);
    }
}