using NUnit.Framework.Constraints;
using UnityEngine;

public class CameraController : MonoBehaviour {
    
    Camera cam;
    float startSize;

    public static CameraController instance;
    void Awake() {
        instance = this;
        cam = GetComponent<Camera>();
        startSize = cam.orthographicSize;
    }
    
    public static Vector3 position {
        get => instance.transform.position;
        set => instance.transform.position = value;
    }
    
    public static float size {
        get => instance.cam.orthographicSize / instance.startSize;
        set {
            if (value < 0.5f) {
                instance.cam.orthographicSize = 0.5f * instance.startSize;
            } else if (value > 2) {
                instance.cam.orthographicSize = 2 * instance.startSize;
            } else {
                instance.cam.orthographicSize = value * instance.startSize;
            }
        }
    }
    
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
