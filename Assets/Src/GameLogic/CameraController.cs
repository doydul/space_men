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
}
