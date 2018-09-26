using UnityEngine;

[RequireComponent(typeof(Tile))]
public class StartLocation : MonoBehaviour {

    private Tile tile;

    public Vector2 gridLocation { get { return tile.gridLocation; } }

    void Awake() {
        tile = GetComponent<Tile>();
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 0.25f);
    }
}
