using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SFXLayer : MonoBehaviour {
    
    public static SFXLayer instance;

    public Transform explosionPrefab;
    public Border borderPrefab;

    public GameObject SpawnExplosion(Vector2 location) => SpawnPrefab(explosionPrefab, location);

    void Awake() {
        instance = this;
    }

    Vector3 Position3D(Vector2 position2D) {
        Vector3 position3D = position2D;
        position3D.z = -4;
        return position3D;
    }
    Vector3 Position3D(Vector3 position3D) => Position3D((Vector2) position3D);

    public GameObject SpawnPrefab(Transform prefab, Vector2 location, Quaternion rotation = default(Quaternion)) {
        var transform = Instantiate(prefab) as Transform;
        transform.position = Position3D(location);
        transform.SetParent(this.transform, true);
        transform.rotation = rotation;
        transform.localScale = Vector3.one;
        return transform.gameObject;
    }

    public void Tracer(Vector3 origin, Vector3 target, Weapon weapon, bool hit, IEnumerable<ParticleBurst> effects = null) => StartCoroutine(PerformTracer(origin, target, weapon, hit, effects));

    public IEnumerator PerformTracer(Vector3 origin, Vector3 target, Weapon weapon, bool hit, IEnumerable<ParticleBurst> effects = null) {
        float randomness = hit ? 0.1f : 0.5f;
        var randomVec = new Vector2(Random.value * randomness * 2 - randomness, Random.value * randomness * 2 - randomness);
        Vector3 targetPos = target + (Vector3)randomVec;
        var tracerObj = SpawnPrefab(weapon.tracerPrefab.transform, origin);
        
        var tracer = tracerObj.GetComponent<Tracer>();
        if (hit) {
            yield return tracer.PerformAnimation(origin, targetPos);
            SpawnBurst(targetPos, origin - targetPos, effects);
        } else {
            RaycastHit raycastHit;
            var ray = new Ray(origin, targetPos - origin);
            bool didHit = Physics.Raycast(
                ray,
                out raycastHit,
                100,
                (1 << LayerMask.NameToLayer("Walls")) | (1 << LayerMask.NameToLayer("Obstacles"))
            );
            var hitPoint = didHit ? raycastHit.point : ray.GetPoint(100);
            yield return tracer.PerformAnimation(
                origin,
                hitPoint
            );
            SpawnBurst(hitPoint, didHit ? raycastHit.normal : origin - targetPos, effects);
        }
    }
    
    public void SpawnBurst(Vector3 position, Vector3 normal, ParticleBurst burstPrefab) {
        if (burstPrefab == null) return;
        var burst = Instantiate(burstPrefab, transform);
        burst.position = Position3D(position);
        burst.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(normal.y, normal.x) * Mathf.Rad2Deg - 90));
    }
    public void SpawnBurst(Vector3 position, Vector3 normal, IEnumerable<ParticleBurst> burstPrefabs) {
        if (burstPrefabs == null) return;
        foreach (var burstPrefab in burstPrefabs) SpawnBurst(position, normal, burstPrefab);
    }
    
    class BorderTile {
        public Vector2 pos;
        public Dictionary<int, Dictionary<int, bool>> nodes;
        
        public bool hasNodes => nodes[-1][1] || nodes[1][1] || nodes[1][-1] || nodes[-1][-1];
        
        public IEnumerable<Vector2> GetNodes() {
            if (nodes[-1][1]) yield return new Vector2(-1, 1);
            if (nodes[1][1]) yield return new Vector2(1, 1);
            if (nodes[1][-1]) yield return new Vector2(1, -1);
            if (nodes[-1][-1]) yield return new Vector2(-1, -1);
        }
        public bool HasNode(Vector2 pos) {
            return nodes[(int)pos.x][(int)pos.y];
        }
        
        public BorderTile() {
            nodes = new();
            nodes[-1] = new Dictionary<int, bool>();
            nodes[1] = new Dictionary<int, bool>();
            nodes[-1][-1] = false;
            nodes[-1][1] = false;
            nodes[1][-1] = false;
            nodes[1][1] = false;
        }
    }
    
    public Border SpawnBorder(List<Vector2> tiles) {
        var orientationMap = new Dictionary<Vector2, Vector2> {
            { new Vector2(-1, 1), new Vector2(0, 1) },
            { new Vector2(1, 1), new Vector2(1, 0) },
            { new Vector2(1, -1), new Vector2(0, -1) },
            { new Vector2(-1, -1), new Vector2(-1, 0) }
        };
        
        Dictionary<Vector2, BorderTile> borderTiles = new();
        foreach (var tilePos in tiles) {
            borderTiles[tilePos] = new BorderTile { pos = tilePos };
        }
        foreach (var borderTile in borderTiles.Values) {
            foreach (var x in new[] { -1, 1 }) {
                foreach (var y in new[] { -1, 1 }) {
                    if (!borderTiles.ContainsKey(new Vector2(x, y) + borderTile.pos)) {
                        borderTile.nodes[x][y] = true;
                    }
                }
            }
        }
        var positions = new List<Vector3>();
        var firstTile = borderTiles.Values.Where(tile => tile.hasNodes).First();
        var firstNode = firstTile.GetNodes().First();
        var activeTile = firstTile;
        var activeNode = firstNode;
        int i = 0;
        while (i < 1000) {
            i++;
            positions.Add(new Vector3(activeTile.pos.x + activeNode.x * 0.4f, activeTile.pos.y + activeNode.y * 0.4f, 0));
            var orientation = orientationMap[activeNode];
            var transpose = new Vector2(orientation.y, -orientation.x);
            if (borderTiles.ContainsKey(activeTile.pos + orientation)) {
                var adjacentTile = borderTiles[activeTile.pos + orientation];
                if (adjacentTile.HasNode(activeNode - orientation * 2)) {
                    activeNode = activeNode - orientation * 2;
                }
                activeTile = adjacentTile;
            } else if (activeTile.HasNode(activeNode + transpose * 2)) {
                activeNode = activeNode + transpose * 2;
            } else if (borderTiles.ContainsKey(activeTile.pos + transpose)) {
                activeTile = borderTiles[activeTile.pos + transpose];
            } else {
                activeNode += transpose * 2;
            }
            if (activeTile == firstTile && activeNode == firstNode) break;
        }
        
        var result = Instantiate(borderPrefab);
        result.transform.parent = transform;
        result.transform.localPosition = Vector3.zero;
        result.SetPoints(positions);
        return result;
    }
}