using UnityEngine;

public abstract class ObjectiveCondition : MonoBehaviour {

    public abstract bool satisfied { get; }

    void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 0.3f);
    }
}