using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using SimplexNoise;

public class HiveMind : MonoBehaviour {
    
    public static HiveMind instance;
    
    public static bool enemiesStartAlerted;
    
    public class Spawning {
        public string type;
        public int number;
    }

    class WeightedTile : IWeighted {
        public int Weight { get; set; }
        public Tile tile { get; set; }
    }

    class WeightedSpawner : IWeighted {
        public int Weight { get; set; }
        public Spawner spawner { get; set; }
    }

    public int threatPerPrimary;
    public int threatPerSecondary;
    public List<EnemySpawnTracker> spawnTrackers = new();
    Alien activeAlien;
    bool threatIncreased;
    
    void Awake() => instance = this;

    public void Init() {
        foreach (var profileWithThreat in Mission.current.enemyProfiles) {
            var profile = profileWithThreat.profile;
            var groupSize = profile.AvailableGroupSize(PlayerSave.current);
            var threat = profileWithThreat.threat;
            spawnTrackers.Add(new EnemySpawnTracker {
                profile = profile,
                startingThreat = threat,
                spawningThreat = threat * profile.spawnPercentage / 100,
                groupSize = groupSize,
                weight = threat * 10 / (groupSize * profile.threat)
            });
        }
        
        int totalThreat = spawnTrackers.Aggregate(0, (agg, tracker) => agg + tracker.initialThreat);
        var threatValues = MathUtil.RandFixedSum(Map.instance.rooms.Count + 3, totalThreat).ToList();
        var locations = Map.instance.EvenlySpacedPoints(new AlienSpawnMask(), threatValues.Count);
        threatValues.Sort(); threatValues.Reverse();
        
        Debug.Log($"threatValues: ${threatValues.Count}");
        Debug.Log($"locations: ${locations.Count}");
        
        int i = 0;
        foreach (var value in threatValues) {
            var threat = value;
            var location = locations[i];
            var nearbyTiles = Map.instance.iterator.Exclude(new JustDoorsPathingMask()).EnumerateFrom(location).Take(15).Select(node => node.tile);
            while (threat > 0) {
                var spawnTracker = spawnTrackers.WeightedSelect();
                InstantiatePod(spawnTracker.type, spawnTracker.groupSize, nearbyTiles.Sample().gridLocation, false);
                threat -= spawnTracker.totalThreatCost;
            }
            i++;
        }

        GameEvents.On(this, "alien_turn_start", AlienTurnStart);
        GameEvents.On(this, "threat_increased", IncreaseThreat);
    }

    void OnDestroy() {
        GameEvents.RemoveListener(this, "alien_turn_start");
        GameEvents.RemoveListener(this, "threat_increased");
    }

    void Update() {
        if (UIState.instance.IsAlienTurn() && !Mission.current.finished) ContemplateMoves();
    }
    
    private void IncreaseThreat() {
        Debug.Log("Increasing threat...");
        threatIncreased = true;
        foreach (var tracker in spawnTrackers) {
            tracker.spawningThreat += tracker.startingThreat / 5;
            Debug.Log($"{tracker.profile.name}, new threat: {tracker.spawningThreat}");
        }
        Debug.Log("Initiating emergency procedure...");
        foreach (var door in Map.instance.GetActors<Door>()) {
            door.Remove();
        }
        foreach (var alien in Map.instance.GetActors<Alien>()) {
            alien.Awaken();
        }
    }

    private void ContemplateMoves() {
        if (!AnimationManager.instance.animationInProgress) {
            if (activeAlien == null) {
                if (!ChooseActiveAlien()) {
                    GameEvents.Trigger("alien_turn_end");
                    UIState.instance.StartPlayerTurn();
                    GameEvents.Trigger("player_turn_start");
                    return;
                }
                AnimationManager.instance.StartAnimation(PerformAlienMove());
            }
        }
    }

    private bool ChooseActiveAlien() {
        var list = Map.instance.GetActors<Alien>().Where(alien => alien.canAct).ToArray();
        if (list.Count() <= 0) return false;
        activeAlien = list.MaxBy(alien => alien.movement);
        return true;
    }

    private IEnumerator PerformAlienMove() {
        yield return activeAlien.behaviour.PerformTurn();
        activeAlien.hasActed = true;
        activeAlien = null;
    }

    private void AlienTurnStart() => Spawn();
    
    public void Spawn(IEnumerable<Spawning> spawnings) {
        foreach (var spawning in spawnings) {
            var weightedSpawners = Map.instance.spawners.Where(spawner => !Map.instance.GetActors<Soldier>().Any(sol => sol.On(spawner.tile)) && Map.instance.GetActors<Soldier>().Select(sol => Map.instance.ManhattanDistance(sol.gridLocation, spawner.gridLocation)).Min() > AlienData.Get(spawning.type).minSpawnDistance).Select(spawner => 
                new WeightedSpawner {
                    spawner = spawner,
                    Weight = Map.instance.GetActors<Soldier>().Select(soldier => Map.instance.ManhattanDistance(soldier.gridLocation, spawner.gridLocation)).Min()
                }
            );
            if (weightedSpawners.Where(wSpawner => wSpawner.spawner.pathable).Any()) weightedSpawners = weightedSpawners.Where(wSpawner => wSpawner.spawner.pathable);
            weightedSpawners = weightedSpawners.OrderBy(wSpawner => wSpawner.Weight).Take(10).ToList();
            int i = 0;
            foreach (var ws in weightedSpawners) {
                ws.Weight = 100 - i * 10;
                i++;
            }
            
            var spawner = weightedSpawners.WeightedSelect().spawner;
            InstantiatePod(spawning.type, spawning.number, spawner.gridLocation, true);
        }
    }
    
    public void SpawnInFog(int minDist, int maxDist, IEnumerable<Spawning> spawnings) {
        var possibleSpawnLocations = new List<Vector2>();
        foreach (var node in Map.instance.iterator.EnumerateFrom(Map.instance.GetActors<Soldier>().Select(sol => sol.gridLocation))) {
            if (node.tile.foggy) {
                node.userData++;
                if (node.userData >= minDist && node.userData <= maxDist) possibleSpawnLocations.Add(node.tile.gridLocation);
            } else if (node.tile.HasActor<Vent>()) {
                possibleSpawnLocations.Add(node.tile.gridLocation);
            }
        }
        foreach (var spawning in spawnings) {
            var spawnLocation = possibleSpawnLocations.Where(pos => Map.instance.GetActors<Soldier>().Select(sol => Map.instance.ManhattanDistance(pos, sol.gridLocation)).Min() >= AlienData.Get(spawning.type).minSpawnDistance).Sample();
            InstantiatePod(spawning.type, spawning.number, spawnLocation, true);
        }
    }

    private void Spawn() {
        var spawnings = new List<Spawning>();
        foreach (var tracker in spawnTrackers) {
            int nominalTurnCount = 12;
            float avgSpawnsPerTurn = ((float)tracker.spawningThreat / tracker.threatCost) / nominalTurnCount;
            if (threatIncreased) avgSpawnsPerTurn = (tracker.startingThreat / 4) / tracker.threatCost;
            
            var stdDev = avgSpawnsPerTurn * 0.4f;
            if (avgSpawnsPerTurn < 0.5f) {
                float t = avgSpawnsPerTurn / 0.5f;
                stdDev = t * stdDev + (1 - t) * avgSpawnsPerTurn * 3;
            }
            int spawns = Mathf.Max(0, (int)Mathf.Round(GaussianNumber.Generate(avgSpawnsPerTurn, stdDev)));
            
            for (int j = 0; j < spawns; j++) {
                spawnings.Add(new Spawning { type = tracker.profile.typeName, number = 1 });
            }
        }
        Spawn(spawnings);
        threatIncreased = false;
    }

    public Alien.Pod InstantiatePod(string type, int number, Vector2 gridLocation, bool awaken) {
        int counter = 0;
        var pod = new Alien.Pod();
        foreach (var node in Map.instance.iterator.Exclude(new AlienImpassableTerrain()).EnumerateFrom(gridLocation)) {
            if (node.tile.occupied) continue;
            var alien = InstantiateAlien(node.tile.gridLocation, type);
            pod.members.Add(alien);
            alien.pod = pod;
            if (awaken) alien.Awaken();
            counter++;
            if (counter >= number) break;
        }
        return pod;
    }

    private Alien InstantiateAlien(Vector2 gridLocation, string alienType) {
        var trans = MonoBehaviour.Instantiate(Resources.Load<Transform>("Prefabs/Alien")) as Transform;

        var alienData = Resources.Load<AlienData>($"Aliens/{alienType}");
        var alien = trans.GetComponent<Alien>() as Alien;
        alienData.Dump(alien);
        alien.id = System.Guid.NewGuid().ToString();
        if (enemiesStartAlerted) alien.Awaken();

        var spriteTransform = Instantiate(Resources.Load<Transform>("Prefabs/AlienSprites/" + alienType + "AlienSprite")) as Transform;
        alien.SetSpriteTransform(spriteTransform);

        Map.instance.GetTileAt(gridLocation).SetActor(trans);
        alien.TurnTo(Actor.RandomDirection());
        return alien;
    }
}