using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityIcon : MonoBehaviour {

    public Image image;
    public SpriteRenderer spriteRenderer;
    public TMP_Text centreTextEl;
    public TMP_Text smallTextEl;
    public Color disabledColor;
    public Action OnClick;
    
    public string centreText { get => centreTextEl.text; set => centreTextEl.text = value; }
    public string smallText { get => smallTextEl.text; set => smallTextEl.text = value; }

    public void DisplaySpriteFor(Ability ability) {
        if (image != null) {
            image.sprite = ability.sprite;
            image.color = ability.spriteColor;
        }
        if (spriteRenderer != null) {
            spriteRenderer.sprite = ability.sprite;
            spriteRenderer.color = ability.spriteColor;
        }
        ability.Display(this);
    }

    public void HandleClick() => OnClick();
    public void Disable() => image.color = disabledColor;
}