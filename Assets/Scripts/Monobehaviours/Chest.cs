using UnityEngine;
using System;
using System.Collections;

public class Chest : Actor {
    
    public GameObject bigChestGraphics;
    public GameObject smallChestGraphics;
    
    public Loot contents;
    GameObject activeGraphic;
    public bool isOpen { get; private set; }
    public Action onOpen { get; set; }
    
    public bool isBig {
        get => bigChestGraphics.activeSelf;
        set {
            bigChestGraphics.SetActive(value);
            smallChestGraphics.SetActive(!value);
            activeGraphic = value ? bigChestGraphics : smallChestGraphics;
        }
    }
    public override bool interactable => !isOpen;
    
    public override IEnumerator PerformUse(Soldier user) {
        yield return GameplayOperations.PerformPickupChest(user, tile);
    }
    
    public void Open() {
        activeGraphic.transform.Find("ClosedSprite").gameObject.SetActive(false);
        activeGraphic.transform.Find("OpenSprite").gameObject.SetActive(true);
        isOpen = true;
        if (onOpen != null) onOpen();
    }
    
    void Start() {
        isBig = contents.hasItem;
        // if (Random.value < 0.5f) transform.localRotation = Quaternion.Euler(0, 0, 90);
    }
}