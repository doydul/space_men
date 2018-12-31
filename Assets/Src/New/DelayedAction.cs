using System;
using UnityEngine;

public class DelayedAction : CustomYieldInstruction {

    public override bool keepWaiting { get {
        return !finished;
    } }

    public static DelayedAction Resolve() {
        var result = new DelayedAction();
        result.Finish();
        return result;
    }

    bool finished;
    Action callback;
    DelayedAction child;

    public void Finish() {
        finished = true;
        Execute();
    }

    public DelayedAction Then(Action callback) {
        this.callback = callback;
        child = new DelayedAction();
        if (finished) Execute();
        return child;
    }

    void Execute() {
        if (callback != null) callback();
        if (child != null) child.Finish();
    }
}
