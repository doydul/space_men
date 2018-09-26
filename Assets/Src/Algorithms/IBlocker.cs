using UnityEngine;

public interface IBlocker {

    float Blockage(Vector2 gridLocation);

    bool ValidTarget(Vector2 gridLocation);
}
