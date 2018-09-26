using UnityEngine;
using UnityEngine.Events;
using System.Collections;

[SelectionBase]
public class Tile : MonoBehaviour {

    public Transform actor;

    public SpriteRenderer backgroundSprite;
    public Transform foreground;
    public SpriteRenderer highlightSprite;
    public SpriteRenderer fogSprite;
    public UnityEvent SoldierEnter;

    public bool open;

    public Vector2 gridLocation;

    public bool foggy { get; private set; }
    public bool occupied { get { return actor != null; } }

    void Awake() {
        fogSprite.color = new Color(1, 1, 1, 0.7f);
        if (SoldierEnter == null) SoldierEnter = new UnityEvent();
    }

    public void SetActor(Transform actor) {
        this.actor = actor;
        actor.GetComponent<Actor>().tile = this;
        actor.parent = foreground;
        actor.localPosition = Vector3.zero;
        actor.localScale = new Vector3(1, 1, 1);
    }

    public void RemoveActor() {
        actor = null;
    }

    public T GetActor<T>() {
        if (actor == null) return default(T);
        return actor.GetComponent<T>();
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
    }

    void OnDrawGizmos() {
        if (SoldierEnter.GetPersistentEventCount() > 0) {
            Gizmos.color = Color.white;
            Gizmos.DrawSphere(transform.position, 0.25f);
        }
    }
}
