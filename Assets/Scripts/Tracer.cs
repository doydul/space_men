using UnityEngine;
using System.Collections;

public class Tracer : MonoBehaviour {

    public float speed;
    public SpriteRenderer sprite;

    public Vector2 realLocation => transform.position;

    public void SetColor(Color color) => sprite.color = color;

    public void Animate(Vector2 from, Vector2 to) => StartCoroutine(PerformAnimation(from, to));

    public IEnumerator PerformAnimation(Vector2 from, Vector2 to) {
        float iterations = (from - to).magnitude / speed;
        for (int i = 0; i < iterations; i++) {
            Vector2 newPos = Vector3.Lerp(from, to, i / iterations);
            transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
            yield return null;
        }
        transform.position = new Vector3(to.x, to.y, transform.position.z);
        yield return null;
        Destroy(gameObject);
    }
}
