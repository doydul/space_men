using System;
using System.Linq;
using Data;
using UnityEngine;
using UnityEngine.UI;

public class AbilityIcon : MonoBehaviour {

    public IconDefinition[] iconDefinitions;

    public Image image;
    public Action OnClick;

    bool disabled;

    public void DisplaySpriteFor(SpecialAbilityType abilityType) {
        Debug.Log(abilityType.ToString());
        image.sprite = iconDefinitions.First(def => def.abilityName == abilityType.ToString()).sprite;
    }

    public void HandleClick() {
        OnClick();
    }

    [System.Serializable]
    public struct IconDefinition {
        public string abilityName;
        public Sprite sprite;
    }
}