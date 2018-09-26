using UnityEngine;

public class MapMarkerController : MonoBehaviour {

    public Map map;
    public Transform explosionMarkerPrefab;

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
}
