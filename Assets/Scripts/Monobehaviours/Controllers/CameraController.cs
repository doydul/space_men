using System.Collections;
using NUnit.Framework.Constraints;
using UnityEngine;

public class CameraController : MonoBehaviour {
    
    public float moveSpeed;
    
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
    public static void CentreCameraOn(Tile tile) {
        if (instance.co != null) instance.StopCoroutine(instance.co);
        instance.co = instance.StartCoroutine(PerformCentreCameraOn(tile));
    }
    public static void CentreCameraOn(Vector2 targetLocation) => CentreCameraOn(Map.instance.GetTileAt(targetLocation));
    public static void CentreCameraOn(Actor actor) => CentreCameraOn(actor.tile);
    
    static IEnumerator PerformCentreCameraOn(Tile tile) {
        var startTime = Time.time;
        var startPos = instance.transform.position;
        var targetPos = startPos;
        targetPos.x = tile.transform.position.x;
        targetPos.y = tile.transform.position.y;
        float duration = (targetPos - startPos).magnitude / instance.moveSpeed;
        
        while (Time.time - startTime < duration) {
            float t = (Time.time - startTime) / duration;
            instance.transform.position = Vector3.Lerp(startPos, targetPos, MathUtil.EaseCubic(t));
            yield return null;
        }
        
        instance.transform.position = targetPos;
    }
}
