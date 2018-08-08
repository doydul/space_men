using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class UIAnimator {
    
    public enum AnimationType {EaseIn, EaseOut, EaseBoth};
        
    private MonoBehaviour context;
    private Action<float> callback;
    private Coroutine co;
    private Queue<Animation> queue;
    private float animationTime;
    
    private float value;
    private Animation currentAnimation;
    
    public bool animating {
        get {
            return co != null;
        }
    }
    
    public UIAnimator(float initialValue, float animationTime, MonoBehaviour context, Action<float> callback) {
        queue = new Queue<Animation>();
        
        this.context = context;
        this.callback = callback;
        this.animationTime = animationTime;
        value = initialValue;
    }
    
    public void Enqueue(float targetValue, Action callback = null, AnimationType type = AnimationType.EaseBoth) {
        var animation = new Animation(targetValue, animationTime, type, callback);
        if (!animating) {
            Animate(animation);
        } else {
            queue.Enqueue(animation);
        }
    }
    
    private void Dequeue() {
        Animate(queue.Dequeue());
    }
    
    private void Animate(Animation nextAnimation) {
        currentAnimation = nextAnimation;
        currentAnimation.startTime = Time.time;
        currentAnimation.startValue = value;
        currentAnimation.change = currentAnimation.targetValue - value;
        if (co != null) context.StopCoroutine(co);
        co = context.StartCoroutine(Process());
    }
    
    private IEnumerator Process() {
        while (true) {
            if (Time.time - currentAnimation.startTime >= animationTime) {
                if (currentAnimation.finishedCallback != null) currentAnimation.finishedCallback();
                co = null;
                if (queue.Count > 0) Dequeue();
                yield break;
            }
            value = currentAnimation.nextValue();
            callback(value);
            yield return null;
        }
    }
    
    private class Animation {
        
        public float startValue;
        public float change;
        public float targetValue;
        public Action finishedCallback;
        public AnimationType type;
        public float startTime;
        public float duration;
        
        public Animation(float targetValue, float duration, AnimationType type, Action finishedCallback) {
            this.targetValue = targetValue;
            this.finishedCallback = finishedCallback;
            this.type = type;
            this.duration = duration;
        }
        
        public float nextValue() {
            if (type == AnimationType.EaseIn) {
                return EaseIn();
            } else if (type == AnimationType.EaseOut) {
                return EaseOut();
            } else if (type == AnimationType.EaseBoth) {
                return EaseBoth();
            }
            return 0f;
        }
        
        private float Progress() {
            return (Time.time - startTime) / duration;
        }
        
        private float EaseIn() {
            return change * Progress() * Progress() * Progress() + startValue;
        }
        
        private float EaseOut() {
            var t = Progress() - 1;
            return change * (t * t * t + 1) + startValue;
        }
        
        private float EaseBoth() {
            var t = Progress() * 2;
            if (t < 1) return change / 2 * t * t * t + startValue;
            t -= 2;
            return change / 2 * (t * t * t + 2) + startValue;
        }
    }
}