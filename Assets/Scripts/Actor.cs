using UnityEngine;
using System;
using System.Collections;

public abstract class Actor : MonoBehaviour {

    public enum Direction {
        Up,
        Left,
        Down,
        Right
    }

    public Tile tile;
    public Transform image;
    public Sprite[] bloodSplatSprites;
    public GameObject hitIndicator;
    public GameObject deflectIndicator;
    public SpriteRenderer healthIndicator;

    public string id { get; set; }
    public long index { get; set; } // remove me
    public Direction direction { get; private set; }
    public bool dead { get; private set; }

    public int maxHealth { get; set; }
    public int health { get; set; }
    public int healthPercentage { get { return health * 100 / maxHealth; } }
    public Vector2 gridLocation { get { return tile.gridLocation; } }
    public Vector2 realLocation { get { return transform.position; } }
    public int rotation { get { return (int)direction * 90; } }
    public Vector2 facing { get {
        return new Vector2(
            (float)Mathf.Sin(rotation * Mathf.PI / 180),
            (float)Mathf.Cos(rotation * Mathf.PI / 180)
        );
    } }

    Coroutine healthCoroutine;
    public void ShowHealth() {
        if (healthCoroutine != null) StopCoroutine(healthCoroutine);
        StartCoroutine(PerformShowHealth());
    }
    public IEnumerator PerformShowHealth() {
        SetHealthIndicatorSize();
        if (healthIndicator.size.x > 0) healthIndicator.enabled = true;
        yield return new WaitForSeconds(1f);
        healthIndicator.enabled = false;
    }

    Coroutine hitCoroutine;
    public void ShowHit() {
        if (hitCoroutine != null) StopCoroutine(hitCoroutine);
        StartCoroutine(PerformShowHit());
    }
    public IEnumerator PerformShowHit() {
        hitIndicator.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        hitIndicator.SetActive(false);
    }
    
    private float healthSpriteSize;

    protected virtual void Awake() {
        health = maxHealth;
        if (healthIndicator != null) {
            healthSpriteSize = healthIndicator.size.x;
            healthIndicator.enabled = false;
        }
        if (hitIndicator != null) hitIndicator.SetActive(false);
    }

    private void SetHealthIndicatorSize() {
        var currentSize = healthIndicator.size;
        currentSize.x = health * healthSpriteSize / maxHealth;
        healthIndicator.size = currentSize;
    }

    public virtual void MoveTo(Tile newTile) {
        tile.RemoveActor(); 
        newTile.SetActor(transform);
    }

    public void TurnTo(Direction direction) {
        this.direction = direction;
        image.rotation = Quaternion.Euler(0, 0, rotation);
    }

    public void TurnTo(Vector2 direction) {
        if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x)) {
            if (direction.y > 0) {
                TurnTo(Direction.Up);
            } else if (direction.y < 0) {
                TurnTo(Direction.Down);
            }
        } else {
            if (direction.x > 0) {
                TurnTo(Direction.Right);
            } else if (direction.x < 0) {
                TurnTo(Direction.Left);
            }
        }
    }

    public void Remove() {
        if (tile.GetActor<Actor>() == this) {
            tile.RemoveActor();
        } else {
            tile.RemoveBackgroundActor();
        }
        Destroy(gameObject, 0.5f);
    }

    public void Hurt(int damage) {
        health -= damage;
        if (health <= 0) {
            dead = true;
            Remove();
        }
        ShowHealth();
    }

    public void Face(Vector2 targetGridLocation) {
        TurnTo(targetGridLocation - gridLocation);
    }

    public virtual void Interact(Tile tile) {}

    public virtual void Select() {}
}
