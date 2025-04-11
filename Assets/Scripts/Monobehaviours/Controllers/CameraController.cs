using System.Collections;
using System.Linq;
using UnityEngine;

public class CameraController : MonoBehaviour {
    
    public float moveDuration;
    
    Camera cam;
    float startSize;

    public static CameraController instance;
    
    public float zoom => cam.orthographicSize;
    
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
    public static void CentreCameraOn(Actor actor, bool snap = false) => CentreCameraOn(actor.realLocation, snap);
    public static void CentreCameraOn(Tile tile, bool snap = false) => CentreCameraOn(tile.realLocation, snap);
    public static void CentreCameraOn(Transform trans, bool snap = false) => CentreCameraOn(trans.position, snap);
    public static void CentreCameraOn(Vector2 targetLocation, bool snap = false) {
        if (instance.co != null) instance.StopCoroutine(instance.co);
        if (snap) {
            var pos = instance.transform.position;
            pos.x = targetLocation.x;
            pos.y = targetLocation.y;
            instance.transform.position = pos;
        } else {
            instance.co = instance.StartCoroutine(PerformCentreCameraOn(targetLocation));
        }
    }
    public static void CentreCameraOnGridLocation(Vector2 targetLocation, bool snap = false) => CentreCameraOn(Map.instance.GetTileAt(targetLocation).realLocation, snap);
    public static void CentreCameraOn(bool snap, params Transform[] transforms) {
        Vector2 avrgPos = transforms.Select(trans => trans.position).Aggregate(Vector3.zero, (agg, next) => agg + next) / transforms.Length;
        Debug.Log(avrgPos);
        CentreCameraOn(avrgPos, snap);
    }
    
    static IEnumerator PerformCentreCameraOn(Vector2 targetLocation) {
        var startTime = Time.time;
        var startPos = instance.transform.position;
        var targetPos = startPos;
        targetPos.x = targetLocation.x;
        targetPos.y = targetLocation.y;
        
        while (Time.time - startTime < instance.moveDuration) {
            float t = (Time.time - startTime) / instance.moveDuration;
            instance.transform.position = Vector3.Lerp(startPos, targetPos, MathUtil.EaseCubic(t));
            yield return null;
        }
        
        instance.transform.position = targetPos;
    }
}
