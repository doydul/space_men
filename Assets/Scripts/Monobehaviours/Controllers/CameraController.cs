using UnityEngine;

public class CameraController : MonoBehaviour {

    public static CameraController instance;
    void Awake() => instance = this;

    public static void CentreCameraOn(Tile tile) {
        var temp = instance.transform.position;
        temp.x = tile.transform.position.x;
        temp.y = tile.transform.position.y;
        instance.transform.position = temp;
    }
    
    public static void CentreCameraOn(Vector2 targetLocation) {
        CentreCameraOn(Map.instance.GetTileAt(targetLocation));
    }
    
    public static void CentreCameraOn(Actor actor) {
        var temp = instance.transform.position;
        temp.x = actor.transform.position.x;
        temp.y = actor.transform.position.y;
        instance.transform.position = temp;
    }
}
