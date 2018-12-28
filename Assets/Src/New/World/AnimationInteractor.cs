using UnityEngine;

public class AnimationInteractor : WorldAnimation.IAnimationInteractor {

    public Transform gunflarePrefab;
    public Transform tracerPrefab;

    public override GameObject MakeGunflare(Vector2 position, float rotation) {
        var spriteTransform = Object.Instantiate(gunflarePrefab) as Transform;
        var spriteObject = spriteTransform.gameObject;
        spriteTransform.position = Position3D(position);
        spriteTransform.transform.eulerAngles = new Vector3(0, 0, rotation);
        return spriteObject;
    }

    public override GameObject MakeTracer(Vector2 start, Vector2 end) {
        var tracerTransform = Object.Instantiate(tracerPrefab) as Transform;
        var tracerObject = tracerTransform.gameObject;
        var tracerScript = tracerTransform.GetComponent<Tracer>();
        tracerTransform.position = Position3D(start);
        tracerScript.StartAnimating(start, end);
        return tracerObject;
    }

    Vector3 Position3D(Vector2 position2D) {
        Vector3 position3D = position2D;
        position3D.z = -4;
        return position3D;
    }
}
