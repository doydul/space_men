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
    public Color disabledColor;
    public Action OnClick;

    public void DisplaySpriteFor(string abilityType) {
        image.sprite = iconDefinitions.First(def => def.abilityName == abilityType).sprite;
    }

    public void HandleClick() => OnClick();
    public void Disable() => image.color = disabledColor;
}