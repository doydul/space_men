using UnityEngine;
using System.Collections;

[SelectionBase]
public class Tile : MonoBehaviour {

    public Transform actor;

    public SpriteRenderer backgroundSprite;
    public Transform foreground;
    public SpriteRenderer highlightSprite;
    public SpriteRenderer fogSprite;

    public bool open;

    public Vector2 gridLocation;

    public bool foggy { get; private set; }
    public bool occupied { get { return actor != null; } }

    void Awake() {
        fogSprite.color = new Color(1, 1, 1, 0.7f);
    }

    public void SetActor(Transform actor) {
        this.actor = actor;
        actor.GetComponent<Actor>().tile = this;
        actor.parent = foreground;
        actor.localPosition = Vector3.zero;
    }

    public void RemoveActor() {
        actor = null;
    }

    public T GetActor<T>() {
        if (actor == null) return default(T);
        return actor.GetComponent<T>();
    }

    public void Highlight() {
        highlightSprite.enabled = true;
    }

    public void ClearHighlight() {
        highlightSprite.enabled = false;
    }

    public void SetFoggy() {
        foggy = true;
        fogSprite.enabled = true;
    }

    public void RemoveFoggy() {
        foggy = false;
        fogSprite.enabled = false;
    }
}
