using UnityEngine;

public class AnimationInteractor : WorldAnimation.IAnimationInteractor {

    public Transform gunflarePrefab;
    public Transform tracerPrefab;
    public Transform explosionCloudPrefab;

    public override GameObject MakeGunflare(Vector2 position, float rotation) {
        var spriteTransform = Instantiate(gunflarePrefab) as Transform;
        var spriteObject = spriteTransform.gameObject;
        spriteTransform.position = Position3D(position);
        spriteTransform.transform.eulerAngles = new Vector3(0, 0, rotation);
        return spriteObject;
    }

    public override Tracer MakeTracer(Vector2 realLocation) {
        var tracerTransform = Instantiate(tracerPrefab) as Transform;
        tracerTransform.position = Position3D(realLocation);
        return tracerTransform.GetComponent<Tracer>();
    }

    public override GameObject MakeExplosionCloud(Vector2 realLocation) {
        var cloudTransform = Instantiate(explosionCloudPrefab) as Transform;
        var cloudObject = cloudTransform.gameObject;
        cloudTransform.position = Position3D(realLocation);
        return cloudObject;
    }

    Vector3 Position3D(Vector2 position2D) {
        Vector3 position3D = position2D;
        position3D.z = -4;
        return position3D;
    }
}
