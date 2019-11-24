using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class GenericObjective : MonoBehaviour {

    public ObjectiveCondition[] conditions;
    public UnityEvent trigger;

    void OnPhaseChange() {
        if (conditions.All((condition => condition.satisfied))) {
            trigger.Invoke();
        }
    }
}