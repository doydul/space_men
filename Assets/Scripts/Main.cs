using UnityEngine;
using System.Collections;
using System.Linq;

public class Main : MonoBehaviour {

    public static Main instance;
    
    public int turnCounter { get; private set; }

    DependencyInjector factory = new();

    void Awake() {
        Application.targetFrameRate = 35;
        instance = this;
    }

    void Start() {
        if (PlayerSave.current == null) {
            var save = PlayerSave.New();
            PlayerSave.current = save;

            var squad = MetaSquad.GenerateDefault();
            save.squad = squad;
            
            Mission.Generate(save);
        }

        MapInstantiator.instance.Generate(PlayerSave.current.difficulty);

        Map.instance.enemyProfiles = Mission.current.enemyProfiles;
        int turnsToCompletion = Map.instance.objectives.objectives.Select(objc => objc.extraTurns).Sum() + Map.instance.objectives.EstimateTravelDistance() / 6;
        TurnCounter.instance.SetTurnLimit(turnsToCompletion);

        int j = 0;
        foreach (var metaSoldier in PlayerSave.current.squad.GetMetaSoldiers()) {
            InstantiateSoldier(metaSoldier, Map.instance.startLocations[j].gridLocation);
            j++;
        }

        FogManager.instance.UpdateFog(true);
        SoldierIconHeader.instance.DisplaySoldiers();
        HiveMind.instance.Init();
        
        StartCoroutine(ShowIntro());
        
        GameEvents.On(this, "player_turn_start", IncrementTurnCounter);
    }
    
    void OnDestroy() {
        GameEvents.RemoveListener(this, "player_turn_start");
    }
    
    void IncrementTurnCounter() {
        turnCounter++;
        if (turnCounter >= 3) {
            Tutorial.Show(GameObject.Find("TurnText").transform, "threat", true, true);
        }
    }
    
    IEnumerator ShowIntro() {
        yield return new WaitForSeconds(3);
        Tutorial.Show("intro");
        var soldiers = Map.instance.GetActors<Soldier>();
        if (soldiers.Where(sol => sol.HasAbility<LayDownFire>()).Any()) {
            Tutorial.Show(soldiers.First(sol => sol.HasAbility<LayDownFire>()).transform, "machine_gun");
        }
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
