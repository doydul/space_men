using UnityEngine;

public class Scripting : MonoBehaviour {
    
    public static Scripting instance { get; private set; }

    void Awake() {
        instance = this;
    }

    public void Trigger(Event e) {
        SendMessage(e.ToString(), null, SendMessageOptions.DontRequireReceiver);
    }

    public enum Event {
        OnMissionStart,
        OnPhaseChange,
        OnThreatEscalation,
        OnSelectSoldier,
        OnMoveSoldier,
        OnTraverse
    }
}