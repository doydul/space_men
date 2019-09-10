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
        
    }
}
