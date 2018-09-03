using UnityEngine;
using System.Collections.Generic;

public class GameplayOrchestrator : MonoBehaviour {

    public Map map;

    void Awake() {
        SpawnSoldiers(Squad.active.soldiers);
    }

    public void SpawnSoldiers(List<SoldierData> soldierDatas) {
        for (int i = 0; i < soldierDatas.Count; i++) {
            Spawn(soldierDatas[i], map.startLocations[i].gridLocation);
        }
    }

    public Soldier Spawn(SoldierData soldierData, Vector2 gridLocation) {
        var trans = Instantiate(Resources.Load<Transform>("Prefabs/Soldier")) as Transform;
        Debug.Log(trans);
        var soldier = trans.GetComponent<Soldier>();
        soldierData.ToSoldier(soldier);

        map.GetTileAt(gridLocation).SetActor(trans);
        return soldier;
    }
}
