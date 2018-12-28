using UnityEngine;
using System.Collections;

public class Tracer : MonoBehaviour {

    public float speed;

    public void StartAnimating(Vector2 from, Vector2 to) {
        StartCoroutine(AnimationRoutine(from, to));
    }

    IEnumerator AnimationRoutine(Vector2 from, Vector2 to) {
        int iteration = 0;
        Vector2 direction = (to - from).normalized;
        float rotation = Vector2.Angle(Vector2.up, direction);
        transform.eulerAngles = new Vector3(0, 0, -rotation);
        while (iteration * speed < (from - to).magnitude) {
            Vector2 newPos = from + iteration * speed * direction;
            transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
            iteration++;
            yield return null;
        }
        Destroy(gameObject);
    }
}
