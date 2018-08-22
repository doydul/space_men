using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class AlienDeployer : MonoBehaviour {

    private const int MIN_SPAWN_DISTANCE = 12;

    public Map map;
    public GamePhase gamePhase;
    public FogController fogController;

    private AlienFrequencyCalculator frequencyCalculator;

    public Spawner[] spawners { get { return map.spawners; } }

    private List<Vector2> soldierLocations { get {
        return map.GetActors<Soldier>().Select(soldier => soldier.gridLocation).ToList();
    } }
    private VirtualMap virtualMap;

    void Awake() {
        virtualMap = new VirtualMap();
        frequencyCalculator = new AlienFrequencyCalculator(new List<AlienFrequencyInput>() {
            new AlienFrequencyInput() {
                alienType = "Alien",
                frequency = 2,
                threat = 1
            }
        });
    }

    void Start() {
        gamePhase.MovementPhaseEnd.AddListener(Iterate);
        fogController.FogChanged.AddListener(SpawnRevealedAliens);
    }

    void Iterate() {
        Spawn();
        CreateNewSpawners();
    }

    void CreateVirtualSpawner(Vector2 gridLocation, ISpawnModule spawnModule) {
        var virtualSpawner = new VirtualSpawner(map, spawnModule, gridLocation);
        virtualMap.AddVirtualSpawner(virtualSpawner);
    }

    void Spawn() {
        foreach (var virtualAlien in virtualMap.virtualAliens) {
            var tile = map.GetTileAt(virtualAlien.gridLocation);
            tile.SetActor(InstantiateAlien(virtualAlien));
        }
        virtualMap.Depopulate();
    }

    void CreateNewSpawners() {
        var availableSpawners = AvailableSpawners();

        var alienProfiles = frequencyCalculator.Iterate();
        foreach (var profile in alienProfiles) {
            while (profile.spawnCount > 0) {
                if (availableSpawners.Count <= 0) break;
                var spawner = availableSpawners[Random.Range (0, availableSpawners.Count)];
                availableSpawners.Remove(spawner);
                if (profile.spawnCount >= 3 && Random.value < 0.5f) {
                    CreateVirtualSpawner(spawner.gridLocation, new GroupSpawnModule(profile.spawnCount));
                    profile.spawnCount = 0;
                } else if (profile.spawnCount >= 3) {
                    CreateVirtualSpawner(spawner.gridLocation, new TrickleSpawnModule(profile.spawnCount));
                    profile.spawnCount = 0;
                } else {
                    CreateVirtualSpawner(spawner.gridLocation, new SingleSpawnModule());
                    profile.spawnCount -= 1;
                }
            }
        }

        virtualMap.Populate(soldierLocations);
    }

    List<Spawner> AvailableSpawners() {
        var result = new List<Spawner>();
        foreach (var spawner in spawners) {
            bool include = true;
            foreach (var soldier in map.GetActors<Soldier>()) {
                var dist = Mathf.Abs(spawner.gridLocation.x - soldier.gridLocation.x) + Mathf.Abs(spawner.gridLocation.y - soldier.gridLocation.y);
                if (dist < MIN_SPAWN_DISTANCE) include = false;
            }
            if (include) result.Add(spawner);
        }
        return result;
    }

    void SpawnRevealedAliens() {
        foreach (var virtualAlien in new List<VirtualAlien>(virtualMap.virtualAliens)) {
            var tile = map.GetTileAt(virtualAlien.gridLocation);
            if (!tile.foggy) {
                virtualMap.virtualAliens.Remove(virtualAlien);
                if (!tile.occupied) {
                    tile.SetActor(InstantiateAlien(virtualAlien));
                }
            }
        }
    }

    Transform InstantiateAlien(VirtualAlien virtualAlien) {
        var result = MonoBehaviour.Instantiate(Resources.Load<Transform>("Prefabs/" + virtualAlien.alienType)) as Transform;
        var alien = result.GetComponent<Alien>();
        var path = new Path(new PathFinder(new AlienPathingWrapper(map), virtualAlien.gridLocation, soldierLocations).FindPath());
        if (path.Count > 1) alien.TurnTo(path.First(2)[1] - virtualAlien.gridLocation);
        return result;
    }
}
