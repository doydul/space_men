using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    public void Tracer(Vector3 origin, Vector3 target, Weapon weapon, bool hit, IEnumerable<ParticleBurst> effects = null) => StartCoroutine(PerformTracer(origin, target, weapon, hit, effects));

    public IEnumerator PerformTracer(Vector3 origin, Vector3 target, Weapon weapon, bool hit, IEnumerable<ParticleBurst> effects = null) {
        float randomness = hit ? 0.1f : 0.5f;
        var randomVec = new Vector2(Random.value * randomness * 2 - randomness, Random.value * randomness * 2 - randomness);
        Vector3 targetPos = target + (Vector3)randomVec;
        var tracerObj = SpawnPrefab(weapon.tracerPrefab.transform, origin);
        
        var tracer = tracerObj.GetComponent<Tracer>();
        if (hit) {
            yield return tracer.PerformAnimation(origin, targetPos);
            SpawnBurst(targetPos, origin - targetPos, effects);
        } else {
            RaycastHit raycastHit;
            var ray = new Ray(origin, targetPos - origin);
            bool didHit = Physics.Raycast(
                ray,
                out raycastHit,
                100,
                (1 << LayerMask.NameToLayer("Walls")) | (1 << LayerMask.NameToLayer("Obstacles"))
            );
            var hitPoint = didHit ? raycastHit.point : ray.GetPoint(100);
            yield return tracer.PerformAnimation(
                origin,
                hitPoint
            );
            SpawnBurst(hitPoint, didHit ? raycastHit.normal : origin - targetPos, effects);
        }
    }
    
    public void SpawnBurst(Vector3 position, Vector3 normal, ParticleBurst burstPrefab) {
        if (burstPrefab == null) return;
        var burst = Instantiate(burstPrefab, transform);
        burst.position = Position3D(position);
        burst.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(normal.y, normal.x) * Mathf.Rad2Deg - 90));
    }
    public void SpawnBurst(Vector3 position, Vector3 normal, IEnumerable<ParticleBurst> burstPrefabs) {
        if (burstPrefabs == null) return;
        foreach (var burstPrefab in burstPrefabs) SpawnBurst(position, normal, burstPrefab);
    }
}