using UnityEngine;
using UnityEngine.VFX;

public class TracerEffect : MonoBehaviour {
    
    public VisualEffect effect;
    
    public void SetPoints(Vector3 start, Vector3 end) {
        transform.position = start;
        effect.SetVector3("EndPoint", end - start);
    }
}