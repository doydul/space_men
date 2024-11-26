using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TracerEffect))]
public class Tracer : MonoBehaviour {

    public float speed;
    public SpriteRenderer sprite;

    public Vector2 realLocation => transform.position;

    public void SetColor(Color color) => sprite.color = color;

    public void Animate(Vector2 from, Vector2 to) => StartCoroutine(PerformAnimation(from, to));
    
    TracerEffect effect;

    public IEnumerator PerformAnimation(Vector2 from, Vector2 to) {
        float iterations = (to - from).magnitude / speed;
        Vector3 direction = (to - from) / iterations;
        for (int i = 0; i < iterations; i++) {
            float t = i / iterations;
            Vector3 newPos = Vector3.Lerp(from, to, t);
            newPos.z = transform.position.z;
            if (iterations - i < 1) {
                effect.SetPoints(newPos, new Vector3(to.x, to.y, newPos.z));
            } else {
                effect.SetPoints(newPos, newPos + direction);
            }
            yield return null;
        }
        Destroy(gameObject);
    }
    
    void Awake() {
        effect = GetComponent<TracerEffect>();
    }
}
