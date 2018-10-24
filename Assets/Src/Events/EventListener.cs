using System;

public class EventListener {

    public string eventName;
    public Action callback;

    public EventListener(string eventName, Action callback) {
        this.eventName = eventName;
        this.callback = callback;
    }
}
