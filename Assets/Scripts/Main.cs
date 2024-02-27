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

    void OnEnable() {
        MapInstantiator.instance.Generate();
        Map.instance.enemyProfiles = EnemyProfileSet.Generate(1);

        var squad = MetaSquad.GenerateDefault();
        int i = 0;

        foreach (var metaSoldier in squad.GetMetaSoldiers()) {
            InstantiateSoldier(metaSoldier, Map.instance.startLocations[i].gridLocation);
            i++;
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
        soldier.maxHealth = 20;
        soldier.health = 20;
        soldier.sightRange = 10;

        foreach (var ability in soldier.weapon.abilities) ability.Attach(soldier);
        foreach (var ability in soldier.armour.abilities) ability.Attach(soldier);

        // soldier.maxAmmo = soldierData.maxAmmo;
        // soldier.exp = soldierData.exp;
        // soldier.maxHealth = soldierData.maxHealth;
        // soldier.health = soldierData.health;
        // soldier.TurnTo((Actor.Direction)soldierData.facing);

        Map.instance.GetTileAt(gridLocation).SetActor(trans);
        CameraController.CentreCameraOn(Map.instance.GetTileAt(gridLocation));
        return soldier;
    }
}
