using UnityEngine;

[RequireComponent(typeof(Animator))]
public class IdleAnimator : MonoBehaviour {
    
    public float animationMinTime;
    public float animationMaxTime;
    
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
        Debug.Log(animator.GetFloat("Speed"));
    }
    
    void Update() {
        if (Time.time - lastAnimationTime >= timeUntilAnimation) {
            animator.SetTrigger($"Idle{Random.Range(1, 4)}");
            lastAnimationTime = Time.time;
            SetTimer();
        }
    }
    
    void SetTimer() {
        timeUntilAnimation = Random.Range(animationMinTime, animationMaxTime);
    }
}