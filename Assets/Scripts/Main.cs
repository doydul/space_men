using UnityEngine;
using System.Collections;
using System.Linq;

public class Main : MonoBehaviour {

    public static Main instance;

    DependencyInjector factory = new();

    void Awake() {
        Application.targetFrameRate = 30;
        instance = this;
    }

    void Start() {
        var squad = MetaSquad.GenerateDefault();
        int i = 0;

        foreach (var metaSoldier in squad.GetMetaSoldiers()) {
            InstantiateSoldier(metaSoldier, Map.instance.startLocations[i].gridLocation);
            i++;
        }

        var openTiles = Map.instance.EnumerateTiles().Where(tile => tile.open).ToArray();
        int totalThreat = 200;
        while (totalThreat > 0) {
            var profile = EnemyProfile.GetAll().Where(prof => prof.difficultyLevel == 1).WeightedSelect();
            int randex = (int)(Random.value * openTiles.Length);
            SpawnPod(profile.typeName, profile.count, openTiles[randex].gridLocation);
            totalThreat -= profile.threat;
        }
        FogManager.instance.UpdateFog(true);
    }

    void SpawnPod(string type, int number, Vector2 gridLocation) {
        int counter = 0;
        var pod = new Alien.Pod();
        foreach (var node in Map.instance.iterator.Exclude(new AlienImpassableTerrain()).EnumerateFrom(gridLocation)) {
            if (node.tile.occupied) continue;
            var alien = InstantiateAlien(node.tile.gridLocation, type);
            pod.members.Add(alien);
            alien.pod = pod;
            counter++;
            if (counter >= number) break;
        }
    }

    // TODO move me somewhere else!
    Soldier InstantiateSoldier(MetaSoldier metaSoldier, Vector2 gridLocation) {
        var trans = Instantiate(Resources.Load<Transform>("Prefabs/Soldier")) as Transform;

        var soldier = trans.GetComponent<Soldier>();
        soldier.id = metaSoldier.id;

        soldier.armour = Armour.Get(metaSoldier.armour.name);
        soldier.weapon = Weapon.Get(metaSoldier.weapon.name);
        // soldier.weapon = Weapon.Get(metaSoldier.weapon.name);
        soldier.maxHealth = 10;
        soldier.health = 10;
        soldier.sightRange = 10;

        foreach (var ability in soldier.weapon.abilities) ability.Attach(soldier);
        foreach (var ability in soldier.armour.abilities) ability.Attach(soldier);

        // soldier.maxAmmo = soldierData.maxAmmo;
        // soldier.exp = soldierData.exp;
        // soldier.maxHealth = soldierData.maxHealth;
        // soldier.health = soldierData.health;
        // soldier.TurnTo((Actor.Direction)soldierData.facing);

        Map.instance.GetTileAt(gridLocation).SetActor(trans);
        return soldier;
    }

    // TODO move me somewhere else!
    Alien InstantiateAlien(Vector2 gridLocation, string alienType) {
        var trans = MonoBehaviour.Instantiate(Resources.Load<Transform>("Prefabs/Alien")) as Transform;

        var alienData = Resources.Load<AlienData>($"Aliens/{alienType}");
        var alien = trans.GetComponent<Alien>() as Alien;
        alienData.Dump(alien);
        alien.id = System.Guid.NewGuid().ToString();

        var spriteTransform = Instantiate(Resources.Load<Transform>("Prefabs/AlienSprites/" + alienType + "AlienSprite")) as Transform;
        spriteTransform.parent = alien.image;
        spriteTransform.localPosition = Vector3.zero;

        Map.instance.GetTileAt(gridLocation).SetActor(trans);
        return alien;
    }
}
