using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class ReachHereObjective : MonoBehaviour {

    public Map map;
    public int soldierCountRequired;
    public SoldierPresenceCondition[] conditions;
    public UnityEvent trigger;

    void OnPhaseChange() {
        if (conditions.Where((condition => condition.satisfied)).Count() >= soldierCountRequired) {
            trigger.Invoke();
        }
    }
}