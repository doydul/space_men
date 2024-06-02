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
        if (PlayerSave.current == null) {
            var save = new PlayerSave();
            PlayerSave.current = save;

            var squad = MetaSquad.GenerateDefault();
            save.squad = squad;
            save.Save(0);
        }

        MapInstantiator.instance.Generate();
        Map.instance.enemyProfiles = EnemyProfileSet.Generate(PlayerSave.current.difficulty);
        foreach (var enemyProfile in Map.instance.enemyProfiles.primaries) PlayerSave.current.alienUnlocks.Unlock(enemyProfile.name);
        Objectives.AddToMap(Map.instance, Map.instance.rooms[1]);

        int j = 0;
        foreach (var metaSoldier in PlayerSave.current.squad.GetMetaSoldiers()) {
            InstantiateSoldier(metaSoldier, Map.instance.startLocations[j].gridLocation);
            j++;
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
        soldier.maxHealth = soldier.armour.maxHealth;
        soldier.health = soldier.armour.maxHealth;
        soldier.sightRange = soldier.armour.sightRange;

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
