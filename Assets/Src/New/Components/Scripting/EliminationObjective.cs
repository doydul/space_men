using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class EliminationObjective : MonoBehaviour {

    public Map map;
    public UnityEvent trigger;

    void OnActorKilled() {
        if (map.GetActors<Alien>().Count <= 0) {
            trigger.Invoke();
        }
    }
}