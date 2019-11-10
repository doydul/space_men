using UnityEngine;
using System.Collections.Generic;

public class RadarBlipLayer : MonoBehaviour {
    
    public Transform radarBlipPrefab;

    List<GameObject> blips;

    void Awake() {
        blips = new List<GameObject>();
    }

    Vector3 Position3D(Vector2 position2D) {
        Vector3 position3D = position2D;
        position3D.z = -3;
        return position3D;
    }

    public void AddBlip(Vector2 location) {
        var transform = Instantiate(radarBlipPrefab) as Transform;
        transform.position = Position3D(location);
        transform.SetParent(this.transform, true);
        blips.Add(transform.gameObject);
    }

    public void ClearBlips() {
        foreach (var blip in blips) {
            Destroy(blip);
        }
        blips.Clear();
    }
}