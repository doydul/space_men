using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BlackFade : MonoBehaviour {
    
    Animator animator;
    Action callback;

    void Awake() {
        animator = GetComponent<Animator>();
    }

    public void BeginFade(Action onFadeFinished) {
        animator.SetTrigger("FadeIn");
        callback = onFadeFinished;
    }

    public void TriggerFadeFinished() {
        callback();
    }
}