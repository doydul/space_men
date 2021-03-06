using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    public UnityEvent action;

    UIAnimator animator;
    float initialScale;
    bool clickDisabled;

    public bool animating {
        get {
            return animator.animating;
        }
    }

    void Start() {
        initialScale = transform.localScale.x;
        animator = new UIAnimator(initialScale, 0.1f, this, (value) => {
            var temp = transform.localScale;
            temp.x = value;
            temp.y = value;
            transform.localScale = temp;
        });
    }

    public void OnPointerDown(PointerEventData eventData) {
        animator.Enqueue(initialScale * 0.95f, null, UIAnimator.AnimationType.EaseOut);
    }

    public void OnPointerUp(PointerEventData eventData) {
        animator.Enqueue(initialScale, () => {
            if (action != null && !clickDisabled) action.Invoke();
        }, UIAnimator.AnimationType.EaseIn);
    }

    public void Enable() {
        clickDisabled = false;
    }

    public void Disable() {
        clickDisabled = true;
        OnPointerUp(null);
    }
}
