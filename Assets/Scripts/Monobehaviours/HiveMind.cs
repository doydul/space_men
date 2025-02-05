using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SimplexNoise;

public class HiveMind : MonoBehaviour {
    
    public static HiveMind instance;
    
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

    public List<EnemySpawnTracker> spawnTrackers = new();
    Alien activeAlien;
    bool threatIncreased;
    
    void Awake() => instance = this;

    public void Init() {
        Noise.Seed = Random.Range(0, 10000);
        var weightedTiles = Map.instance.EnumerateTiles().Where(tile => tile.open).Select(tile => {
            var wTile = new WeightedTile { tile = tile, Weight = (int)Mathf.Ceil(Mathf.Pow(Noise.CalcPixel2D((int)tile.gridLocation.x, (int)tile.gridLocation.y, 0.03f) / 255, 3) * 100) };
            var minDist = Map.instance.startLocations.Select(st => Map.instance.ManhattanDistance(st.gridLocation, tile.gridLocation)).Min();
            if (minDist < 10) wTile.Weight = (int)Mathf.Max(wTile.Weight - 100 + minDist * 10, 0);
            // // Debug
            // MapHighlighter.instance.HighlightTile(tile, new Color(wTile.Weight / 100f, wTile.Weight / 100f, wTile.Weight / 100f));
            // //
            return wTile;
        });

        foreach (var profile in Map.instance.enemyProfiles.primaries) {
            spawnTrackers.Add(new EnemySpawnTracker {
                profile = profile,
                remainingThreat = 200 / Map.instance.enemyProfiles.primaries.Count,
                startingThreat = 200 / Map.instance.enemyProfiles.primaries.Count
            });
            Debug.Log($"Initialising primary spawn tracker (profile: {profile.name}, threat: {spawnTrackers[spawnTrackers.Count - 1].remainingThreat})");
        }
        foreach (var profile in Map.instance.enemyProfiles.secondaries) {
            spawnTrackers.Add(new EnemySpawnTracker {
                profile = profile,
                remainingThreat = 33,
                startingThreat = 33
            });
            Debug.Log($"Initialising secondary spawn tracker (profile: {profile.name}, threat: {spawnTrackers[spawnTrackers.Count - 1].remainingThreat})");
        }

        weightedTiles = weightedTiles.Where(wTile => Map.instance.startLocations.Select(st => Map.instance.ManhattanDistance(st.gridLocation, wTile.tile.gridLocation)).Min() > 3).ToList();
        int tileCount = weightedTiles.Count();

        foreach (var tracker in spawnTrackers) {
            int threatPortion = (int)(tracker.remainingThreat * (100f - tracker.profile.spawnPercentage) / 100f);
            tracker.remainingThreat -= threatPortion;
            while (threatPortion > 0) {
                var wTile = weightedTiles.SampleWithCount(tileCount);
                int groupSize = tracker.profile.AvailableGroupSize(Map.instance.enemyProfiles);
                InstantiatePod(tracker.profile.typeName, groupSize, wTile.tile.gridLocation, false);
                threatPortion -= tracker.profile.threat * groupSize;
            }
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
            tracker.remainingThreat += tracker.startingThreat / 5;
            Debug.Log($"{tracker.profile.name}, new threat: {tracker.remainingThreat}");
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
        if (activeAlien == null) {
            if (!ChooseActiveAlien()) {
                UIState.instance.StartPlayerTurn();
                GameEvents.Trigger("player_turn_start");
                return;
            }
            if (!AnimationManager.instance.animationInProgress) AnimationManager.instance.StartAnimation(PerformAlienMove());
        }
    }

    private bool ChooseActiveAlien() {
        var list = Map.instance.GetActors<Alien>().Where(alien => alien.canAct).ToArray();
        if (list.Count() <= 0) return false;
        activeAlien = list[Random.Range(0, list.Count())];
        return true;
    }

    private IEnumerator PerformAlienMove() {
        yield return activeAlien.behaviour.PerformTurn();
        activeAlien.hasActed = true;
        activeAlien = null;
    }

    private void AlienTurnStart() => Spawn();
    
    public void Spawn(IEnumerable<Spawning> spawnings) {
        var weightedSpawners = Map.instance.spawners.Where(spawner => !Map.instance.GetActors<Soldier>().Any(sol => sol.On(spawner.tile))).Select(spawner => 
            new WeightedSpawner {
                spawner = spawner,
                Weight = Map.instance.GetActors<Soldier>().Select(soldier => Map.instance.ManhattanDistance(soldier.gridLocation, spawner.gridLocation)).Min()
            }
        );
        weightedSpawners = weightedSpawners.OrderBy(wSpawner => wSpawner.Weight).Take(10).ToList();
        int i = 0;
        foreach (var ws in weightedSpawners) {
            ws.Weight = 100 - i * 10;
            i++;
        }
        foreach (var spawning in spawnings) {
            var spawner = weightedSpawners.WeightedSelect().spawner;
            InstantiatePod(spawning.type, spawning.number, spawner.gridLocation, true);
        }
    }

    private void Spawn() {
        var spawnings = new List<Spawning>();
        foreach (var tracker in spawnTrackers) {
            int nominalTurnCount = 12;
            float avgSpawnsPerTurn = ((float)tracker.remainingThreat / tracker.profile.threat) / nominalTurnCount;
            if (threatIncreased) avgSpawnsPerTurn = (tracker.startingThreat / 4) / tracker.profile.threat;
            int spawns = (int)Mathf.Round(GaussianNumber.Generate(avgSpawnsPerTurn, Mathf.Max(avgSpawnsPerTurn * 0.45f, 0.5f)));
            Debug.Log($"{tracker.profile.name} avg spawns: {avgSpawnsPerTurn}, spawns: {spawns}");
            for (int j = 0; j < spawns; j++) {
                spawnings.Add(new Spawning { type = tracker.profile.typeName, number = 1 });
            }
        }
        Spawn(spawnings);
        threatIncreased = false;
    }

    public Alien.Pod InstantiatePod(string type, int number, Vector2 gridLocation, bool awaken) {
        int counter = 0;
        Debug.Log($"spawning pod [{number} {type}]");
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

        var spriteTransform = Instantiate(Resources.Load<Transform>("Prefabs/AlienSprites/" + alienType + "AlienSprite")) as Transform;
        alien.SetSpriteTransform(spriteTransform);

        Map.instance.GetTileAt(gridLocation).SetActor(trans);
        return alien;
    }
}