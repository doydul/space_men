using Data;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ShipAbilityButton : MonoBehaviour {
    
    public Image image;
    public Sprite teleportSoldierInSprite;
    public Sprite teleportAmmoInSprite;
    public GameObject greyOverlay;

    public Action OnClick;

    bool disabled;

    public void DisplaySpriteFor(ShipAbilityType abilityType) {
        switch(abilityType) {
            case ShipAbilityType.TeleportSoldierIn:
                image.sprite = teleportSoldierInSprite;
                break;

            case ShipAbilityType.TeleportAmmoIn:
                image.sprite = teleportAmmoInSprite;
                break;
        }
    }

    public void Disable() {
        greyOverlay.SetActive(true);
        disabled = true;
    }

    public void Enable() {
        greyOverlay.SetActive(false);
        disabled = false;
    }

    public void HandleClick() {
        if (!disabled) OnClick();
    }
}