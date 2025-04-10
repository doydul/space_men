using System;
using UnityEngine;
using UnityEngine.UI;

public class AbilityIcon : MonoBehaviour {

    public Image image;
    public SpriteRenderer spriteRenderer;
    public Color disabledColor;
    public Action OnClick;

    public void DisplaySpriteFor(Ability ability) {
        if (image != null) {
            image.sprite = ability.sprite;
            image.color = ability.spriteColor;
        }
        if (spriteRenderer != null) {
            spriteRenderer.sprite = ability.sprite;
            spriteRenderer.color = ability.spriteColor;
        }
    }

    public void HandleClick() => OnClick();
    public void Disable() => image.color = disabledColor;
}