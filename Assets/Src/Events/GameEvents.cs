using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class GameEvents : MonoBehaviour {

    public static GameEvents instance;

    private List<EventListener> listeners = new();

    void Awake() {
        instance = this;
    }

    public static void On(string eventName, Action callback) {}

    public static void On(object self, string eventName, Action callback) {
        instance.InstanceOn(self, eventName, callback);
    }

    public static void RemoveListener(object self, string eventName) {
        instance.InstanceRemoveListener(self, eventName);
    }

    public static void Trigger(string eventName) {
        instance.InstanceTrigger(eventName);
    }

    void InstanceOn(object self, string eventName, Action callback) {
        listeners.Add(new EventListener(self, eventName, callback));
    }

    void InstanceTrigger(string eventName) {
        foreach (var listener in listeners) {
            if (listener.eventName == eventName) listener.callback();
        }
    }

    void InstanceRemoveListener(object self, string eventName) {
        listeners.RemoveAll(listener => listener.self == self && listener.eventName == eventName);
    }
}
