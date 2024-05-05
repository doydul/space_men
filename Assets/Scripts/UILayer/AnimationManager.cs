using UnityEngine;
using System.Collections;

public class AnimationManager : MonoBehaviour {
    
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

    private IEnumerator AnimationRoutineWrapper(IEnumerator animation) {
        animationInProgress = true;
        yield return StartCoroutine(animation);
        animationInProgress = false;
    }
}