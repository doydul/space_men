using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class ExtractionObjective : MonoBehaviour {

    public Map map;
    public SoldierPresenceCondition[] conditions;
    public UnityEvent trigger;

    void OnPhaseChange() {
        if (conditions.Where((condition => condition.satisfied)).Count() >= map.GetActors<Soldier>().Count) {
            trigger.Invoke();
        }
    }
}