using UnityEngine;

public class DisableObject : MonoBehaviour {
    
    public void Activate() {
        gameObject.SetActive(false);
    }
}