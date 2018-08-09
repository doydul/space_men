using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class AlienDeployer : MonoBehaviour {

    public Map map;
    public GamePhase gamePhase;
    public FogController fogController;

    public Spawner[] spawners { get { return map.spawners; } }

    private VirtualMap virtualMap;

    void Awake() {
        virtualMap = new VirtualMap();
    }

    void Start() {
        CreateVirtualSpawner(spawners[0].gridLocation);
        virtualMap.Populate(map.GetActors<Soldier>().Select(soldier => soldier.gridLocation).ToList());
        gamePhase.MovementPhaseEnd.AddListener(Iterate);
        fogController.FogChanged.AddListener(SpawnRevealedAliens);
    }

    void Iterate() {
        Spawn();
        CreateNewSpawners();
    }

    void CreateVirtualSpawner(Vector2 gridLocation) {
        var spawnModule = new SingleSpawnModule();
        var virtualSpawner = new VirtualSpawner(map, spawnModule, gridLocation);
        virtualMap.AddVirtualSpawner(virtualSpawner);
    }

    void Spawn() {
        foreach (var virtualAlien in virtualMap.virtualAliens) {
            var tile = map.GetTileAt(virtualAlien.gridLocation);
            tile.SetActor(InstantiateAlien("Alien"));
        }
        virtualMap.Depopulate();
    }

    void CreateNewSpawners() {
        // create some spawners
        virtualMap.Populate(map.GetActors<Soldier>().Select(soldier => soldier.gridLocation).ToList());
    }

    void SpawnRevealedAliens() {
        foreach (var virtualAlien in new List<VirtualAlien>(virtualMap.virtualAliens)) {
            var tile = map.GetTileAt(virtualAlien.gridLocation);
            if (!tile.foggy) {
                virtualMap.virtualAliens.Remove(virtualAlien);
                if (!tile.occupied) {
                    tile.SetActor(InstantiateAlien("Alien"));
                }
            }
        }
    }

    Transform InstantiateAlien(string type) {
        return MonoBehaviour.Instantiate(Resources.Load<Transform>("Prefabs/" + type)) as Transform;
    }
}
