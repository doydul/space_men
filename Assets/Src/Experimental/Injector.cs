using UnityEngine;

public class Injector : MonoBehaviour {

    public Object target;
    public Object assignee;
    public string fieldName;

    void Awake() {
        assignee.GetType().GetField(fieldName).SetValue(assignee, target);
    }
}
