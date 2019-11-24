using UnityEngine;
using UnityEngine.Events;

public class SoldierTraverseTrigger : MonoBehaviour {

    public UnityEvent callback;

    void OnTraverse() {
        callback.Invoke();
    }
}