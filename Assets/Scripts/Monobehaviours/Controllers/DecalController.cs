using UnityEngine;

public class DecalController : MonoBehaviour {

    public static DecalController instance;

    public Map map;

    void Awake() {
        instance = this;
    }
    
    public void SpawnDecal(Tile tile, Decal decalPrefab) {
        var decal = Instantiate(decalPrefab, transform);
        decal.localPosition = (Vector3)tile.gridLocation;
    }
}
