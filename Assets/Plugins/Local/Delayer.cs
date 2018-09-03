using UnityEngine;
using System.Collections;
using System;

public class Delayer {

    private MonoBehaviour context;
    private Action callback;
    private Coroutine co;

    public Delayer(MonoBehaviour context) {
        this.context = context;
    }

    public void Wait(float time, Action callback) {
        this.callback = callback;
        if (co != null) context.StopCoroutine(co);
        co = context.StartCoroutine(Process(time));
    }

    private IEnumerator Process(float time) {
        yield return new WaitForSeconds(time);
        callback();
    }
}
