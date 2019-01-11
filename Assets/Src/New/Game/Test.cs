using UnityEngine;

public class Test : MonoBehaviour {

    void Start() {
        var pather = new AlienPathFinder(new FakeGrid());

        Debug.Log(pather.ClosestTargetLocation(new Vector2(0, 1), 10));
        Debug.Log(pather.ClosestTargetLocation(new Vector2(2, 6), 10));
    }
}

public class FakeGrid : IAlienGrid {

    bool[,] should;
    bool[,] target;
    bool[,] valid;

    public FakeGrid() {
        should = new bool[8,4] {
            {false, true, false, false},
            {true, true, true, true},
            {false, true, false, false},
            {false, true, true, true},
            {false, false, false, true},
            {false, false, true, true},
            {false, false, true, false},
            {false, false, true, true}
        };

        target = new bool[8,4] {
            {false, false, false, false},
            {false, false, false, true},
            {false, false, false, false},
            {false, false, false, false},
            {false, false, false, true},
            {false, false, false, false},
            {false, false, false, false},
            {false, false, false, true}
        };

        valid = new bool[8,4] {
            {false, true, false, false},
            {true, true, false, false},
            {false, true, false, false},
            {false, true, true, true},
            {false, false, false, true},
            {false, false, true, true},
            {false, false, true, false},
            {false, false, false, false}
        };
    }

    public bool ShouldIterate(Vector2 gridLocation) {
        var x = (int)gridLocation.x;
        var y = (int)gridLocation.y;
        if (x < 0 || x >= 4 || y < 0 || y >= 8) return false;
        return should[(int)gridLocation.y, (int)gridLocation.x];
    }

    public bool IsTargetLocation(Vector2 gridLocation) {
        return target[(int)gridLocation.y, (int)gridLocation.x];
    }

    public bool IsValidFinishLocation(Vector2 gridLocation) {
        return valid[(int)gridLocation.y, (int)gridLocation.x];
    }
}
