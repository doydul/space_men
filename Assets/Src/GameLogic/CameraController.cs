using UnityEngine;

public class CameraController : ICameraController {

    public CameraController(Camera cam) {
        this.cam = cam;
    }

    Camera cam;

    public void CentreCameraOn(Tile tile) {
        var temp = cam.transform.position;
        temp.x = tile.transform.position.x;
        temp.y = tile.transform.position.y;
        cam.transform.position = temp;
    }
}
