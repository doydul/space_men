using UnityEngine;
using UnityEngine.Events;

public class SoldierTraverseTrigger : MonoBehaviour {

    public bool oneTime = true;
    public UnityEvent trigger;

    bool hasTriggered;

    void OnTraverse() {
        if (!hasTriggered || !oneTime) {
            trigger.Invoke();
            hasTriggered = true;
        }
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 0.5f);
    }
}