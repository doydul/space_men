using UnityEngine;

public class VirtualAlien {

    public Vector2 gridLocation;
    public Actor.Direction direction;

    public VirtualAlien(Vector2 gridLocation, Actor.Direction direction) {
        this.gridLocation = gridLocation;
        this.direction = direction;
    }
}
