using UnityEngine;
using System.Collections.Generic;

public class GameInitializer {

    public GameInitializer(Map map, FogController fogController, AlienDeployer alienDeployer, RadarBlipController radarBlipController) {
        this.map = map;
        this.fogController = fogController;
        this.alienDeployer = alienDeployer;
        this.radarBlipController = radarBlipController;
    }

    Map map;
    FogController fogController;
    AlienDeployer alienDeployer;
    RadarBlipController radarBlipController;

    public void Init() {
        SpawnSoldiers(Squad.GenerateDefault()._activeSoldiers);
        fogController.Recalculate();
        alienDeployer.Iterate();
        radarBlipController.ShowRadarBlips();
        MapHack.Init(map);
    }

    void SpawnSoldiers(List<SoldierData> soldierDatas) {
        SoldierDataHack.Init();
        for (int i = 0; i < soldierDatas.Count; i++) {
            SoldierDataHack.soldiers.Add(
                Spawn(soldierDatas[i], map.startLocations[i].gridLocation, i)
            );
        }
    }

    Soldier Spawn(SoldierData soldierData, Vector2 gridLocation, int index) {
        var trans = MonoBehaviour.Instantiate(Resources.Load<Transform>("Prefabs/Soldier")) as Transform;

        var soldier = trans.GetComponent<Soldier>();
        soldier.FromData(soldierData, index);

        map.GetTileAt(gridLocation).SetActor(trans);
        return soldier;
    }
}
