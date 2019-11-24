using UnityEngine;
using UnityEngine.Events;

public class SoldierTraverseTrigger : MonoBehaviour {

    public UnityEvent trigger;

    void OnTraverse() {
        trigger.Invoke();
    }
}