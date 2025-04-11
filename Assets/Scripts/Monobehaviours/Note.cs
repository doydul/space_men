using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Note : MonoBehaviour {
    
    public float duration;
    public float fadeDuration;
    public TMP_Text textEl;
    
    Vector3 startScale;
    float startTime;
    Image[] images;
    
    void Awake() {
        startTime = Time.time;
        images = GetComponentsInChildren<Image>();
    }
    
    void Update() {
        transform.localScale = Vector3.one * CameraController.instance.zoom;
        
        if (Time.time - startTime > duration) {
            float t = (Time.time - startTime - duration) / fadeDuration;
            if (t >= 1) {
                Destroy(gameObject);
                return;
            }
            var tmp = textEl.color;
            tmp.a = 1 - t;
            textEl.color = tmp;
            foreach (var image in images) {
                tmp = image.color;
                tmp.a = 1 - t;
                image.color = tmp;
            }
        }
    }
    
    public void SetPositionAndText(Vector2 pos, string text) {
        transform.position = pos;
        var tmp = transform.localPosition;
        tmp.z = 0;
        transform.localPosition = tmp;
        textEl.text = text;
    }
}