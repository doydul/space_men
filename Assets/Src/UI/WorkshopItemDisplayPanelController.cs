using UnityEngine;
using UnityEngine.UI;
using System;

public class WorkshopItemDisplayPanelController : MonoBehaviour {

    public Image itemPortrait;

    private Action<InventoryItem> callback;
    private InventoryItem item;

    public void SetCallback(Action<InventoryItem> callback) {
        this.callback = callback;
    }

    public void SetItem(InventoryItem item) {
        itemPortrait.enabled = item != null;
        this.item = item;
        itemPortrait.sprite = new SpriteSelector(item).Select();
    }

    public void InvokeCallback() {
        callback(item);
    }
}
