using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AbilityIcon : MonoBehaviour {

    [System.Serializable]
    public struct IconDefinition {
        public string abilityName;
        public Sprite sprite;
    }

    public IconDefinition[] iconDefinitions;
    public Image image;
    public SpriteRenderer spriteRenderer;
    public Color disabledColor;
    public Action OnClick;

    public void DisplaySpriteFor(string abilityType) {
        var sprite = iconDefinitions.First(def => def.abilityName == abilityType).sprite;
        if (image != null) image.sprite = sprite;
        if (spriteRenderer != null) spriteRenderer.sprite = sprite;
    }

    public void HandleClick() => OnClick();
    public void Disable() => image.color = disabledColor;
}