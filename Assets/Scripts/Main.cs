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
        for (int j = 0; j < 30; j++) {
            int randex = (int)(Random.value * openTiles.Length);
            if (openTiles[randex].GetActor<Actor>() == null) {
                InstantiateAlien(openTiles[randex].gridLocation);
            }
        }
        FogManager.instance.UpdateFog(true);
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

        // soldier.maxAmmo = soldierData.maxAmmo;
        // soldier.exp = soldierData.exp;
        // soldier.maxHealth = soldierData.maxHealth;
        // soldier.health = soldierData.health;
        // soldier.TurnTo((Actor.Direction)soldierData.facing);

        Map.instance.GetTileAt(gridLocation).SetActor(trans);
        return soldier;
    }

    // TODO move me somewhere else!
    Alien InstantiateAlien(Vector2 gridLocation) {
        var trans = MonoBehaviour.Instantiate(Resources.Load<Transform>("Prefabs/Alien")) as Transform;
        var type = "Alien";

        var alien = trans.GetComponent<Alien>() as Alien;
        alien.id = System.Guid.NewGuid().ToString();

        var spriteTransform = Instantiate(Resources.Load<Transform>("Prefabs/AlienSprites/" + type + "AlienSprite")) as Transform;
        spriteTransform.parent = alien.image;
        spriteTransform.localPosition = Vector3.zero;
        alien.type = type;
        alien.damage = 3;
        alien.movement = 6;
        alien.expReward = 1;
        alien.maxHealth = 7;
        alien.health = 7;
        alien.sensoryRange = 10;

        Map.instance.GetTileAt(gridLocation).SetActor(trans);
        return alien;
    }
}
