using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(VisualEffect))]
public class ParticleBurst : MonoBehaviour {
    
    VisualEffect effect;
    
    public Vector3 position {
        get => transform.position;
        set => transform.position = value;
    }
    
    public Quaternion rotation {
        get => transform.rotation;
        set => transform.rotation = value;
    }
    
    public void SetEffect(VisualEffectAsset asset) {
        effect.visualEffectAsset = asset;
    }
    
    void Awake() {
        effect = GetComponent<VisualEffect>();
        Destroy(gameObject, 5);
    }
}