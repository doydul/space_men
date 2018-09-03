using UnityEngine;

[RequireComponent(typeof(Tile))]
public class Spawner : MonoBehaviour {
    
    public Tile tile { get; set; }
    
    public Vector2 gridLocation { get { return tile.gridLocation; } }
    
    void Awake() {
        tile = GetComponent<Tile>();
    }
    
    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.25f);
    }
}