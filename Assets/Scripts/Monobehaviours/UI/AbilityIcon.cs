using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AbilityIcon : MonoBehaviour {

    public Image image;
    public SpriteRenderer spriteRenderer;
    public Color disabledColor;
    public Action OnClick;

    public void DisplaySpriteFor(Ability ability) {
        var sprite = ability.sprite;
        if (image != null) image.sprite = sprite;
        if (spriteRenderer != null) spriteRenderer.sprite = sprite;
    }

    public void HandleClick() => OnClick();
    public void Disable() => image.color = disabledColor;
}