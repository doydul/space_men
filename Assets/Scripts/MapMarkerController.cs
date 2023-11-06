using UnityEngine;
using System.Collections.Generic;

public class MapMarkerController : MonoBehaviour {

    public static MapMarkerController instance;

    public Map map;
    public Transform explosionMarkerPrefab;
    public Transform radarBlipPrefab;

    private List<GameObject> radarBlips;

    void Awake() {
        radarBlips = new List<GameObject>();
        instance = this;
    }

    public void CreateExplosionMarker(Tile tile, float opacity) {
        var transform = Instantiate(explosionMarkerPrefab) as Transform;
        var temp = transform.GetComponent<SpriteRenderer>().color;
        temp.a = opacity;
        transform.GetComponent<SpriteRenderer>().color = temp;
        transform.position = tile.transform.position + new Vector3(0, 0, -3);
        transform.parent = map.transform;
        Destroy(transform.gameObject, 1);
    }

    public void CreateExplosionMarker(Vector2 gridLocation, float opacity) {
        CreateExplosionMarker(map.GetTileAt(gridLocation), opacity);
    }

    public void CreateRadarBlip(Vector2 gridLocation) {
        var transform = Instantiate(radarBlipPrefab) as Transform;
        transform.parent = map.transform;
        transform.position = map.GetTileAt(gridLocation).transform.position + new Vector3(0, 0, -3);
        radarBlips.Add(transform.gameObject);
    }

    public void ClearRadarBlips() {
        foreach (var blip in radarBlips) {
            Destroy(blip);
        }
    }
}
