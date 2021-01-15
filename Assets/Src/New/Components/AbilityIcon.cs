using System;
using Data;
using UnityEngine;
using UnityEngine.UI;

public class AbilityIcon : MonoBehaviour {
    
    public Image image;
    public Sprite fireAtGroundSprite;
    public Sprite grenadeSprite;
    public Sprite collectAmmoSprite;
    public Sprite stunShotSprite;

    public Action OnClick;

    bool disabled;

    public void DisplaySpriteFor(SpecialAbilityType abilityType) {
        switch(abilityType) {
            case SpecialAbilityType.FireAtGround:
                image.sprite = fireAtGroundSprite;
                break;

            case SpecialAbilityType.Grenade://
                image.sprite = grenadeSprite;
                break;

            case SpecialAbilityType.CollectAmmo:
                image.sprite = collectAmmoSprite;
                break;

            case SpecialAbilityType.StunShot:
                image.sprite = stunShotSprite;
                break;
        }
    }

    public void HandleClick() {
        OnClick();
    }
}