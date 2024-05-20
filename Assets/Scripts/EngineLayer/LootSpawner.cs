using UnityEngine;

[RequireComponent(typeof(Tile))]
public class LootSpawner : MonoBehaviour {

    private Tile tile;

    public Vector2 gridLocation { get {
        if (tile == null) tile = GetComponent<Tile>();
        return tile.gridLocation;
    } }

    void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.25f);
    }
}
