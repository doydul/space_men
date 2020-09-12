using Data;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ShipAbilityButton : MonoBehaviour {
    
    public Image image;
    public Sprite teleportSoldierInSprite;
    public Sprite teleportAmmoInSprite;

    public Action OnClick;

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

    public void HandleClick() {
        OnClick();
    }
}