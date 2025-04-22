using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class AnimationManager : MonoBehaviour {
    
    class CoroutineData {
        public IEnumerator enumerator;
        public bool finished;
    }
    
    public bool animationInProgress { get; private set; }

    public static AnimationManager instance;
    
    void Start() {
        instance = this;
    }

    public void StartAnimation(IEnumerator animation) {
        if (animationInProgress) {
            Debug.LogError("Trying to start an animation while one is already in progress. Should have checked the value of animationInProgress");
        } else {
            StartCoroutine(AnimationRoutineWrapper(animation));
        }
    }
    
    public IEnumerator WaitForAll(params IEnumerator[] enumerators) {
        var coroutines = new List<CoroutineData>();
        foreach (var enumerator in enumerators) {
            var data = new CoroutineData() { enumerator = enumerator };
            coroutines.Add(data);
            StartCoroutine(PerformCoroutineAsync(data));
        }
        while (coroutines.Any()) {
            yield return null;
            coroutines.RemoveAll(data => data.finished);
        }
    }
    
    public static void Delay(float seconds, Action callback) {
        instance.StartCoroutine(PerformDelay(seconds, callback));
    }
    
    private static IEnumerator PerformDelay(float seconds, Action callback) {
        yield return new WaitForSeconds(seconds);
        callback();
    }

    private IEnumerator AnimationRoutineWrapper(IEnumerator animation) {
        animationInProgress = true;
        MapHighlighter.instance.ClearHighlights();
        yield return StartCoroutine(animation);
        if (UIState.instance.IsActorSelected()) UIState.instance.GetSelectedActor().Select();
        animationInProgress = false;
    }
    
    private IEnumerator PerformCoroutineAsync(CoroutineData data) {
        yield return data.enumerator;
        data.finished = true;
    }
}