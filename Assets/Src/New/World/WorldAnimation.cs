using System;
using System.Collections;
using UnityEngine;

public abstract class WorldAnimation {

    public abstract IEnumerator Animate(IAnimationInteractor interactor, DelayedAction delayedAction);

    public abstract class IAnimationInteractor : MonoBehaviour {

        public abstract GameObject MakeGunflare(Vector2 position, float rotation);

        public abstract GameObject MakeTracer(Vector2 start, Vector2 end);
    }
}
