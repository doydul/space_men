using UnityEngine;
using System.Collections.Generic;

public class FogGrid {

    private List<Vector2> targets;
    private int radius;

    public FogGrid(List<Vector2> targets, int radius) {
        this.targets = targets;
        this.radius = radius;
    }

    public bool InFog(Vector2 gridLocation) {
        foreach (var target in targets) {
            var dist = Mathf.Abs(target.x - gridLocation.x) + Mathf.Abs(target.y - gridLocation.y);
            if (dist <= radius) return false;
        }
        return true;
    }
}
