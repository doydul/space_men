using UnityEngine;

[RequireComponent(typeof(Animator))]
public class IdleAnimator : MonoBehaviour {
    
    public float animationMinTime;
    public float animationMaxTime;
    public int idleAnimations = 4;
    
    Animator animator;
    float lastAnimationTime;
    float timeUntilAnimation;
    
    void Awake() {
        animator = GetComponent<Animator>();
        SetTimer();
    }
    
    void Start() {
        animator.SetFloat("Speed", Random.Range(0.9f, 1.1f));
        animator.SetFloat("Offset", Random.value);
    }
    
    void Update() {
        if (Time.time - lastAnimationTime >= timeUntilAnimation) {
            animator.SetTrigger($"Idle{Random.Range(1, idleAnimations + 1)}");
            lastAnimationTime = Time.time;
            SetTimer();
        }
    }
    
    void SetTimer() {
        timeUntilAnimation = Random.Range(animationMinTime, animationMaxTime);
    }
}