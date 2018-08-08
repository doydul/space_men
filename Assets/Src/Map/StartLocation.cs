using UnityEngine;

[RequireComponent(typeof(Tile))]
public class StartLocation : MonoBehaviour {
    
    private Tile tile;
    
    void Awake() {
        tile = GetComponent<Tile>();
    }
    
    void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 0.25f);
    }
    
    public Soldier Spawn(SoldierData soldierData) {
        var trans = Instantiate(Resources.Load<Transform>("Prefabs/Soldier")) as Transform;
        var soldier = trans.GetComponent<Soldier>();
        soldierData.ToSoldier(soldier);
        
        tile.SetActor(trans);
        return soldier;
    }
}