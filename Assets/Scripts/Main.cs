using UnityEngine;
using System.Collections;
using System.Linq;

public class Main : MonoBehaviour {

    public static Main instance;

    DependencyInjector factory = new();

    void Awake() {
        Application.targetFrameRate = 35;
        instance = this;
    }

    void OnEnable() {
        if (PlayerSave.current == null) {
            var save = new PlayerSave();
            PlayerSave.current = save;

            var squad = MetaSquad.GenerateDefault();
            save.squad = squad;
            save.Save(0);
            
            Mission.Generate();
        }

        MapInstantiator.instance.Generate(PlayerSave.current.difficulty);

        Map.instance.enemyProfiles = Mission.current.enemyProfiles;
        int turnsToCompletion = Map.instance.objectives.EstimateTravelDistance() / (5 - (int)PlayerSave.current.difficulty);
        int turnsToThreatIncrease = turnsToCompletion / 2;
        TurnCounter.instance.SetTurnLimit(turnsToThreatIncrease);

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
