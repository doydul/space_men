using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameplayOrchestrator : MonoBehaviour {

    public Map map;

    public GameLogicComponent gameLogic;
    public GameEvents events;
    public WorldComponent world;
    public UI ui;

    void Awake() {
        gameLogic.SetWorld(world);
        gameLogic.SetUserInterface(ui);
    }

    void Start() {
        SpawnSoldiers(Squad.GenerateDefault().soldiers);
    }

    public void SpawnSoldiers(List<SoldierData> soldierDatas) {
        for (int i = 0; i < soldierDatas.Count; i++) {
            Spawn(soldierDatas[i], map.startLocations[i].gridLocation);
        }
    }

    public Soldier Spawn(SoldierData soldierData, Vector2 gridLocation) {
        var trans = Instantiate(Resources.Load<Transform>("Prefabs/Soldier")) as Transform;

        var soldier = trans.GetComponent<Soldier>();
        soldier.FromData(soldierData);

        map.GetTileAt(gridLocation).SetActor(trans);
        return soldier;
    }

    public void EndMission() {
        ui.FadeToBlack(() => {
            Squad.IncrementMission();
            if (Squad.currentMission != null) {
                SceneManager.LoadScene("MissionOverview");
            } else {
                SceneManager.LoadScene("MainMenu");
            }
        });
    }
}
