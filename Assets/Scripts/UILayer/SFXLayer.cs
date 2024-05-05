using UnityEngine;

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

    public GameObject SpawnPrefab(Transform prefab, Vector2 location, Quaternion rotation = default(Quaternion)) {
        var transform = Instantiate(prefab) as Transform;
        transform.position = Position3D(location);
        transform.SetParent(this.transform, true);
        transform.rotation = rotation;
        return transform.gameObject;
    }
}