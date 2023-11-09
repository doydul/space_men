using UnityEngine;
using System;

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
        Destroy(gameObject);
    }

    public void Hurt(int damage) {
        health -= damage;
        if (health <= 0) {
            dead = true;
            Remove();
        }
    }

    public void Die(float timer = 0) {
        dead = true;
        tile.RemoveActorByIndex(index);
        Scripting.instance.Trigger(Scripting.Event.OnActorKilled);
        Destroy(this.gameObject, timer);
    }

    public void Face(Vector2 targetGridLocation) {
        TurnTo(targetGridLocation - gridLocation);
    }

    public virtual void Interact(Tile tile) {}

    public virtual void Select() {}
}
