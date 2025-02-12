using UnityEngine;

public class StatusIcon : MonoBehaviour {
    
    public SpriteRenderer spriteRenderer;
    
    public void DisplaySpriteFor(StatusEffect status) {
        spriteRenderer.sprite = status.sprite;
        spriteRenderer.color = status.tint;
    }
}