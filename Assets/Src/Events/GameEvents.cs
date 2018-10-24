using UnityEngine;
using System;
using System.Collections.Generic;

public class GameEvents : MonoBehaviour {

    public static GameEvents instance;

    private List<EventListener> listeners;

    void Awake() {
        instance = this;
        listeners = new List<EventListener>();
    }

    public static void On(string eventName, Action callback) {
        instance.InstanceOn(eventName, callback);
    }

    public static void Trigger(string eventName) {
        instance.InstanceTrigger(eventName);
    }

    void InstanceOn(string eventName, Action callback) {
        listeners.Add(new EventListener(eventName, callback));
    }

    void InstanceTrigger(string eventName) {
        foreach (var listener in listeners) {
            if (listener.eventName == eventName) listener.callback();
        }
    }
}
