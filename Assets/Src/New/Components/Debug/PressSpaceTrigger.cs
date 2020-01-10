using UnityEngine;
using UnityEngine.Events;

public class PressSpaceTrigger : MonoBehaviour {
    
    public UnityEvent trigger;

    void Update() {
        if (Input.GetKeyDown("space")) {
            trigger.Invoke();
        }
    }
}