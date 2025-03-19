using UnityEngine;
using static MapGenerator;

public class RoomTemplateTile : MonoBehaviour {
    public bool isPort;
    public bool isAlienSpawner;
    public bool isPlayerSpawner;
    public bool isLootSpawner;
    public Facing portDirection;
    public bool ignoreOverlap;

    public MapPoint point => new MapPoint((int)Mathf.Round(transform.localPosition.x), (int)Mathf.Round(transform.localPosition.y));

    void OnDrawGizmos() {
        if (isPort) {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position, 0.5f);
        } else if (isAlienSpawner) {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, 0.5f);
        } else if (isPlayerSpawner) {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.position, 0.5f);
        } else if (isLootSpawner) {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, 0.4f);
        }
    }
}