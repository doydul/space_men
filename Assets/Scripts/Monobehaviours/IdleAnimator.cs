using UnityEngine;

[RequireComponent(typeof(Animator))]
public class IdleAnimator : MonoBehaviour {
    
    Animator animator;
    float lastAnimationTime;
    float timeUntilAnimation;
    
    void Awake() {
        animator = GetComponent<Animator>();
        SetTimer();
    }
    
    void Update() {
        if (Time.time - lastAnimationTime >= timeUntilAnimation) {
            animator.SetTrigger("Idle1");
            lastAnimationTime = Time.time;
            SetTimer();
        }
    }
    
    void SetTimer() {
        timeUntilAnimation = Random.value * 100 + 5;
    }
}