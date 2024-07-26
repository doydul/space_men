using UnityEngine;
using UnityEngine.Events;
using System.Collections;

[SelectionBase]
public class Tile : MonoBehaviour {

    public Transform actor;
    public Transform backgroundActor;

    public SpriteRenderer backgroundSprite;
    public Transform foreground;
    public Transform midground;
    public SpriteRenderer highlightSprite;
    public SpriteRenderer fogSprite;
    public UnityEvent SoldierEnter;

    public bool open;

    public Vector2 gridLocation;

    public Fire fire { get; private set; }
    public bool onFire => fire != null;

    public bool foggy { get; private set; }
    public bool occupied { get { return actor != null; } }
    public Vector2 realLocation { get { return transform.position; } }

    public void Init() {
        fogSprite.color = new Color(1, 1, 1, 0.7f);
        if (SoldierEnter == null) SoldierEnter = new UnityEvent();
    }

    public void SetFire(Fire fire) {
        this.fire = fire;
        fire.tile = this;
        fire.transform.parent = midground;
        fire.transform.localPosition = Vector2.zero;
    }

    public void SetActor(Transform actor, bool background = false) {
        if (!background && this.actor != null) Debug.Log("Omg what are you doing!");
        if (background) {
            backgroundActor = actor;
            actor.parent = midground;
        } else {
            this.actor = actor;
            actor.parent = foreground;
        }
        actor.GetComponent<Actor>().tile = this;
        actor.localPosition = Vector3.zero;
        actor.localScale = new Vector3(1, 1, 1);
    }

    public void SetTint(Color color) => backgroundSprite.color = color;

    public void RemoveActor() {
        actor = null;
    }

    public void RemoveBackgroundActor() {
        backgroundActor = null;
    }

    public void RemoveActorByIndex(long index) {
        if (occupied && actor.GetComponent<Actor>().index == index) {
            RemoveActor();
        } else {
            RemoveBackgroundActor();
        }
    }

    public T GetActor<T>() {
        if (actor == null) return default(T);
        return actor.GetComponent<T>();
    }
    
    public bool HasActor<T>() => GetActor<T>() != null;
    
    public void HideBackground() => midground.gameObject.SetActive(false);

    public T GetBackgroundActor<T>() {
        if (backgroundActor == null) return default(T);
        return backgroundActor.GetComponent<T>();
    }

    public void Highlight(Color color) {
        highlightSprite.color = color;
        highlightSprite.enabled = true;
    }

    public void ClearHighlight() {
        highlightSprite.enabled = false;
    }

    public void SetFoggy() {
        foggy = true;
        fogSprite.enabled = true;
        foreground.gameObject.SetActive(false);
    }

    public void RemoveFoggy() {
        foggy = false;
        fogSprite.enabled = false;
        foreground.gameObject.SetActive(true);
        midground.gameObject.SetActive(true);
    }

    void OnDrawGizmos() {
        if (SoldierEnter == null) SoldierEnter = new UnityEvent();
        if (SoldierEnter.GetPersistentEventCount() > 0) {
            Gizmos.color = Color.white;
            Gizmos.DrawSphere(transform.position, 0.25f);
        }
    }
}