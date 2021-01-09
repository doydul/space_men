using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class ReachHereObjective : MonoBehaviour {

    public Map map;
    public int soldierCountRequired;
    public SoldierPresenceCondition[] conditions;
    public UnityEvent trigger;

    bool triggered;

    void OnPhaseChange() {
        if (!triggered && conditions.Where((condition => condition.satisfied)).Count() >= soldierCountRequired) {
            trigger.Invoke();
            triggered = true;
        }
    }
}