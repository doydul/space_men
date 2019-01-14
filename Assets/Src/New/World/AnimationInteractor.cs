using UnityEngine;

public class AnimationInteractor : WorldAnimation.IAnimationInteractor {

    public Transform gunflarePrefab;
    public Transform tracerPrefab;
    public Transform tracerImpactPrefab;
    public Transform explosionCloudPrefab;
    public Transform alienAttackMarkerPrefab;

    public override GameObject MakeGunflare(Vector2 position, float rotation) {
        var spriteTransform = Instantiate(gunflarePrefab) as Transform;
        var spriteObject = spriteTransform.gameObject;
        spriteTransform.position = Position3D(position);
        spriteTransform.transform.eulerAngles = new Vector3(0, 0, rotation);
        return spriteObject;
    }

    public override Tracer MakeTracer(Vector2 realLocation) {
        return MakePrefab(tracerPrefab, realLocation).GetComponent<Tracer>();
    }

    public override GameObject MakeTracerImpact(Vector2 realLocation) {
        return MakePrefab(tracerImpactPrefab, realLocation);
    }

    public override GameObject MakeExplosionCloud(Vector2 realLocation) {
        return MakePrefab(explosionCloudPrefab, realLocation);
    }

    public override GameObject MakeAlienAttackMarker(Vector2 realLocation) {
        return MakePrefab(alienAttackMarkerPrefab, realLocation);
    }

    GameObject MakePrefab(Transform prefab, Vector2 realLocation) {
        var transform = Instantiate(prefab) as Transform;
        transform.position = Position3D(realLocation);
        return transform.gameObject;
    }

    Vector3 Position3D(Vector2 position2D) {
        Vector3 position3D = position2D;
        position3D.z = -4;
        return position3D;
    }
}
