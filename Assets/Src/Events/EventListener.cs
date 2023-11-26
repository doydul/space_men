using System;

public class EventListener {

    public object self;
    public string eventName;
    public Action callback;

    public EventListener(object self, string eventName, Action callback) {
        this.self = self;
        this.eventName = eventName;
        this.callback = callback;
    }
}
