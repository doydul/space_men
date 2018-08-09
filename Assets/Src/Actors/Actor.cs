using UnityEngine;
using System;

public class Actor : MonoBehaviour {

    public enum Direction {
        Up,
        Left,
        Down,
        Right
    }

    public Tile tile;
    public Transform image;
    public Sprite[] bloodSplatSprites;

    public Direction direction { get; private set; }
    public bool dead { get; private set; }

    public Vector2 gridLocation { get { return tile.gridLocation; } }

    public virtual void MoveTo(Tile newTile) {
        tile.RemoveActor();
        newTile.SetActor(transform);
    }

    public void TurnTo(Direction direction) {
        this.direction = direction;
        image.rotation = Quaternion.Euler(0, 0, (int)direction * 90);
    }

    public void TurnTo(Vector2 direction) {
        Face(gridLocation + direction);
    }

    public void Die(float timer = 0) {
        dead = true;
        tile.RemoveActor();
        Destroy(this.gameObject, timer);
    }

    public void Face(Vector2 targetGridLocation) {
        var diff = targetGridLocation - gridLocation;
        if (diff.y > 0) {
            TurnTo(Direction.Up);
        } else if (diff.y < 0) {
            TurnTo(Direction.Down);
        } else if (diff.x > 0) {
            TurnTo(Direction.Right);
        } else if (diff.x < 0) {
            TurnTo(Direction.Left);
        }
    }
}
