using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Tile))]
public class Spawner : MonoBehaviour {
    
    public Tile tile { get; set; }
    public bool pathable { get; private set; }
    
    public Vector2 gridLocation { get { return tile.gridLocation; } }
    
    void Awake() {
        tile = GetComponent<Tile>();
    }
    
    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.5f);
    }
    
    void Start() {
        GameEvents.On(this, "alien_turn_start", CheckPathing);
    }

    void OnDestroy() {
        GameEvents.RemoveListener(this, "alien_turn_start");
    }
    
    void CheckPathing() {
        var soldierPositions = Map.instance.GetActors<Soldier>().Select(soldier => soldier.gridLocation).ToArray();
        var path = Map.instance.ShortestPath(new AlienImpassableTerrain(), tile.gridLocation, soldierPositions, true);
        pathable = path.exists;
    }
}
