using UnityEngine;

public class BloodSplatController : MonoBehaviour {
    
    private const float LOCATION_VARIATION = 0.3f;
    
    public Map map;
    public Transform bloodSplatContainer;
    
    public void MakeSplat(Actor actor) {
        var bloodObject = new GameObject();
        var bloodSpriteRenderer = bloodObject.AddComponent<SpriteRenderer>() as SpriteRenderer;
        var sprite = actor.bloodSplatSprites[Random.Range(0, actor.bloodSplatSprites.Length)];
        bloodSpriteRenderer.sprite = sprite;
        bloodObject.transform.localScale = SpriteScale(sprite, RandomizedSize());
        
        bloodObject.transform.parent = bloodSplatContainer;
        bloodObject.transform.rotation = RandomizedRotation();
        bloodObject.transform.localPosition = RandomizedLocation(actor.gridLocation);
    }
    
    private Vector3 SpriteScale(Sprite sprite, float size) {
        float xScale = sprite.pixelsPerUnit / sprite.rect.width;
        float yScale = sprite.pixelsPerUnit / sprite.rect.height;
        return new Vector3(xScale * size, yScale * size, 1);
    }
    
    private Vector3 RandomizedLocation(Vector2 gridLocation) {
        var randomTranslation = new Vector3(
            Random.value * LOCATION_VARIATION * 2 - LOCATION_VARIATION,
            Random.value * LOCATION_VARIATION * 2 - LOCATION_VARIATION,
            0
        );
        return (Vector3)gridLocation + randomTranslation;
    }
    
    private Quaternion RandomizedRotation() {
        return Quaternion.Euler(0, 0, Random.value * 360);
    }
    
    private float RandomizedSize() {
        return Random.value * 0.25f + 0.75f;
    }
}