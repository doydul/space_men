using UnityEngine;
using System.Linq;

public class AlienDeployer : MonoBehaviour {
    
    public Map map;
    
    public Spawner[] spawners { get { return map.spawners; } }
    
    private VirtualMap virtualMap;
    
    void Awake() {
        virtualMap = new VirtualMap();
        MovementPhase.PhaseEnd += Iterate;
    }
    
    public void Iterate() {
        Spawn();
        CreateNewSpawners();
    }
    
    void Start() {
        CreateVirtualSpawner(spawners[0].gridLocation);
        virtualMap.Populate(map.GetActors<Soldier>().Select(soldier => soldier.gridLocation).ToList());
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
    
    Transform InstantiateAlien(string type) {
        return MonoBehaviour.Instantiate(Resources.Load<Transform>("Prefabs/" + type)) as Transform;
    }
    
    void OnDestroy() {
        MovementPhase.PhaseEnd -= Iterate;
    }
}