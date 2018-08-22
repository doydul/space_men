using UnityEngine;

public class MapMover : MonoBehaviour {

    public Transform mapTransform;
    public IMouseDragManager inputManager;

    void Start() {
        inputManager.MouseDragged += MoveMap;
    }

    void MoveMap(Vector2 translation) {
        mapTransform.position += (Vector3)translation * 0.01f;
    }
}
