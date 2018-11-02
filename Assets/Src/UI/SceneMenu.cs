using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

public abstract class SceneMenu : MonoBehaviour {

    public Image blackFade;

    private UIAnimator fadeAnimator;

    void Awake() {
        fadeAnimator = new UIAnimator(1f, 0.5f, this, (value) => {
            var temp = blackFade.color;
            temp.a = value;
            blackFade.color = temp;
        });

        blackFade.enabled = true;

        _Awake();
    }

    void Start() {
        fadeAnimator.Enqueue(0f, () => {
            blackFade.enabled = false;
        });

        _Start();
    }

    protected void FadeToBlack(Action callback) {
        blackFade.enabled = true;
        fadeAnimator.Enqueue(1f, callback);
    }

    protected virtual void _Awake() {}

    protected virtual void _Start() {}
}
