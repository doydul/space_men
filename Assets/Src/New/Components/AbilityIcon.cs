using System;
using Data;
using UnityEngine;
using UnityEngine.UI;

public class AbilityIcon : MonoBehaviour {
    
    public Image image;
    public Sprite fireAtGroundSprite;
    public Sprite collectAmmoSprite;

    public Action OnClick;

    bool disabled;

    public void DisplaySpriteFor(SpecialActionType abilityType) {
        switch(abilityType) {
            case SpecialActionType.FireAtGround:
                image.sprite = fireAtGroundSprite;
                break;

            case SpecialActionType.CollectAmmo:
                image.sprite = collectAmmoSprite;
                break;
        }
    }

    public void HandleClick() {
        OnClick();
    }
}