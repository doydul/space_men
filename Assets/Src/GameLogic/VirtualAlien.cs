using UnityEngine;

public class VirtualAlien {

    public Vector2 gridLocation;
    public string alienType;
    public float radarPresence;

    public VirtualAlien(string alienType, Vector2 gridLocation) {
        this.gridLocation = gridLocation;
        this.alienType = alienType;
        radarPresence = Random.value;
    }
}
