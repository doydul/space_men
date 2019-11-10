using UnityEngine;

public class HealthBar : MonoBehaviour {
    
    public SpriteRenderer barSprite;

    float startingSize;

    void Awake() {
        startingSize = barSprite.size.x;
    }

    public void SetPercentage(int percentage) {
        barSprite.size = new Vector2(startingSize * percentage / 100, barSprite.size.y);
    }
}