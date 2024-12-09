using UnityEngine;

public class Decal : MonoBehaviour {
    
    public float locationRandomization;
    public bool randomRotation = true;
    
    public Vector3 localPosition {
        get => transform.localPosition;
        set {
            transform.localPosition = new Vector3(
                value.x + Random.value * locationRandomization * 2 - locationRandomization,
                value.y+ Random.value * locationRandomization * 2 - locationRandomization,
                0
            );
            if (randomRotation) transform.rotation = Quaternion.Euler(0, 0, Random.value * 360);
        }
    }
}