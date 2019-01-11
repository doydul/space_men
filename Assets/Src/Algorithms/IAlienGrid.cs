using UnityEngine;

public interface IAlienGrid : IIterableGrid {
    
    bool IsTargetLocation(Vector2 gridLocation);
    bool IsValidFinishLocation(Vector2 gridLocation);
}
