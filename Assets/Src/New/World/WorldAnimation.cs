using System;
using System.Collections;
using UnityEngine;

public abstract class WorldAnimation {

    public abstract IEnumerator Animate(IAnimationInteractor interactor, DelayedAction delayedAction);

    public abstract class IAnimationInteractor : MonoBehaviour {

        public abstract GameObject MakeGunflare(Vector2 position, float rotation);

        public abstract Tracer MakeTracer(Vector2 realLocation, string weaponName);

        public abstract GameObject MakeTracerImpact(Vector2 realLocation);

        public abstract GameObject MakeExplosionCloud(Vector2 realLocation);

        public abstract GameObject MakeAlienAttackMarker(Vector2 realLocation);
    }
}
