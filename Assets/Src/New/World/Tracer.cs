using UnityEngine;
using System.Collections;

public class Tracer : MonoBehaviour {

    public float speed;

    public Vector2 realLocation { get { return transform.position; } }

    public DelayedAction StartAnimating(Vector2 from, Vector2 to) {
        speed = 1f;
        var result = new DelayedAction();
        StartCoroutine(AnimationRoutine(from, to, result));
        return result;
    }

    public void Destroy() {
        Destroy(gameObject);
    }

    IEnumerator AnimationRoutine(Vector2 from, Vector2 to, DelayedAction delayedAction) {
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
        delayedAction.Finish();
    }
}
