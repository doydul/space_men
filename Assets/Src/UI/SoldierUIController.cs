using UnityEngine;
using System;

[RequireComponent(typeof(Soldier))]
public class SoldierUIController : MonoBehaviour {
    
    public SpriteRenderer moveIndicator;
    public GameObject ammoIndicator;
    
    void Awake() {
        HideAll();
    }

    public void SetAmmoCount(int count) {
        ammoIndicator.SetActive(true);
        var ammoTransform = ammoIndicator.transform;
        for (int i = 0; i < ammoTransform.childCount; i++) {
            ammoTransform.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < count; i++) {
            ammoTransform.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void SetMoved() {
        moveIndicator.enabled = true;
        moveIndicator.color = Color.yellow;
    }

    public void SetSprinted() {
        moveIndicator.enabled = true;
        moveIndicator.color = Color.red;
    }

    public void HideAll() {
        moveIndicator.enabled = false;
        ammoIndicator.SetActive(false);
    }
}