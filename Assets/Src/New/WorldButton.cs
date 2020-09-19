using Data;
using UnityEngine;

public class WorldButton : MonoBehaviour {

    void Start() {
        Hide();
    }
    
    public void Show(Tile tile) {
        gameObject.SetActive(true);
        transform.position = tile.realLocation + new Vector2(0, 0.3f);
        var pos = transform.localPosition;
        pos.z = 0;
        transform.localPosition = pos;
    }

    public void Hide() {
        gameObject.SetActive(false);
    }
}