using UnityEngine;

public class HealthIndicator : MonoBehaviour {
    
    public SpriteRenderer fillSprite;
    
    float spriteStartSize;
    
    void Awake() {
        spriteStartSize = fillSprite.size.x;
        Hide();
    }
    
    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);
    
    public void Set(float percentage) {
        var currentSize = fillSprite.size;
        currentSize.x = Mathf.Max(percentage * spriteStartSize, 0);
        fillSprite.size = currentSize;
    }
    public void Set(float max, float current) => Set(current / max);
}