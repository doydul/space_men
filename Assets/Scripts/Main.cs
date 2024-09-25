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

    void Start() {
        if (PlayerSave.current == null) {
            var save = new PlayerSave();
            PlayerSave.current = save;

            var squad = MetaSquad.GenerateDefault();
            save.squad = squad;
            save.Save(0);
            
            save.mission = Mission.Generate();
        }

        MapInstantiator.instance.Generate(PlayerSave.current.difficulty);

        Map.instance.enemyProfiles = Mission.current.enemyProfiles;
        int turnsToCompletion = Map.instance.objectives.EstimateTravelDistance() / 6;
        TurnCounter.instance.SetTurnLimit(turnsToCompletion);

        int j = 0;
        foreach (var metaSoldier in PlayerSave.current.squad.GetMetaSoldiers()) {
            InstantiateSoldier(metaSoldier, Map.instance.startLocations[j].gridLocation);
            j++;
        }

        FogManager.instance.UpdateFog(true);
        SoldierIconHeader.instance.DisplaySoldiers();
        HiveMind.instance.Init();
    }

    // TODO move me somewhere else!
    Soldier InstantiateSoldier(MetaSoldier metaSoldier, Vector2 gridLocation) {
        var trans = Instantiate(Resources.Load<Transform>("Prefabs/Soldier")) as Transform;

        var soldier = trans.GetComponent<Soldier>();
        metaSoldier.Dump(soldier);

        Map.instance.GetTileAt(gridLocation).SetActor(trans);
        CameraController.CentreCameraOn(Map.instance.GetTileAt(gridLocation));
        return soldier;
    }
}
