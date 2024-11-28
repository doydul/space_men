using UnityEngine;
using System.Collections;
using UnityEngine.VFX;

public class SFXLayer : MonoBehaviour {
    
    public static SFXLayer instance;

    public Transform explosionPrefab;

    public GameObject SpawnExplosion(Vector2 location) => SpawnPrefab(explosionPrefab, location);

    void Awake() {
        instance = this;
    }

    Vector3 Position3D(Vector2 position2D) {
        Vector3 position3D = position2D;
        position3D.z = -4;
        return position3D;
    }
    Vector3 Position3D(Vector3 position3D) => Position3D((Vector2) position3D);

    public GameObject SpawnPrefab(Transform prefab, Vector2 location, Quaternion rotation = default(Quaternion)) {
        var transform = Instantiate(prefab) as Transform;
        transform.position = Position3D(location);
        transform.SetParent(this.transform, true);
        transform.rotation = rotation;
        transform.localScale = Vector3.one;
        return transform.gameObject;
    }

    public void Tracer(Vector3 origin, Vector3 target, Weapon weapon, bool hit, VisualEffectAsset effect = null) => StartCoroutine(PerformTracer(origin, target, weapon, hit, effect));

    public IEnumerator PerformTracer(Vector3 origin, Vector3 target, Weapon weapon, bool hit, VisualEffectAsset effect = null) {
        float randomness = hit ? 0.1f : 0.5f;
        var randomVec = new Vector2(Random.value * randomness * 2 - randomness, Random.value * randomness * 2 - randomness);
        Vector3 targetPos = target + (Vector3)randomVec;
        var tracerObj = SpawnPrefab(weapon.tracerPrefab.transform, origin);
        
        var tracer = tracerObj.GetComponent<Tracer>();
        if (hit) {
            yield return tracer.PerformAnimation(origin, targetPos);
            SpawnBurst(targetPos, origin - targetPos, effect);
        } else {
            RaycastHit raycastHit;
            var ray = new Ray(origin, targetPos - origin);
            bool didHit = Physics.Raycast(
                ray,
                out raycastHit, 100, (1 << LayerMask.NameToLayer("Walls"))
            );
            var hitPoint = didHit ? raycastHit.point : ray.GetPoint(100);
            yield return tracer.PerformAnimation(
                origin,
                hitPoint
            );
            SpawnBurst(hitPoint, didHit ? raycastHit.normal : origin - targetPos, effect);
        }
    }
    
    public void SpawnBurst(Vector3 position, Vector3 normal, VisualEffectAsset visualEffect) {
        if (visualEffect == null) return;
        var burst = Instantiate(Resources.Load<ParticleBurst>("Prefabs/SFX/Burst"), transform);
        burst.position = Position3D(position);
        burst.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(normal.y, normal.x) * Mathf.Rad2Deg - 90));
        burst.SetEffect(visualEffect);
    }
}