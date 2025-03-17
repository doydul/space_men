using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour {
    
    public float moveDuration;
    
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
    
    Coroutine co;
    public static void CentreCameraOn(Vector2 targetLocation, bool snap = false) => CentreCameraOn(Map.instance.GetTileAt(targetLocation), snap);
    public static void CentreCameraOn(Actor actor, bool snap = false) => CentreCameraOn(actor.tile, snap);
    public static void CentreCameraOn(Tile tile, bool snap = false) => CentreCameraOn(tile.transform, snap);
    public static void CentreCameraOn(Transform trans, bool snap = false) {
        if (instance.co != null) instance.StopCoroutine(instance.co);
        if (snap) {
            var pos = instance.transform.position;
            pos.x = trans.position.x;
            pos.y = trans.position.y;
            instance.transform.position = pos;
        } else {
            instance.co = instance.StartCoroutine(PerformCentreCameraOn(trans));
        }
    }
    
    static IEnumerator PerformCentreCameraOn(Transform trans) {
        var startTime = Time.time;
        var startPos = instance.transform.position;
        var targetPos = startPos;
        targetPos.x = trans.position.x;
        targetPos.y = trans.position.y;
        
        while (Time.time - startTime < instance.moveDuration) {
            float t = (Time.time - startTime) / instance.moveDuration;
            instance.transform.position = Vector3.Lerp(startPos, targetPos, MathUtil.EaseCubic(t));
            yield return null;
        }
        
        instance.transform.position = targetPos;
    }
}
