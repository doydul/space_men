using UnityEngine;

public class RadarBlipController {

    public RadarBlipController(AlienDeployer alienDeployer) {
        this.alienDeployer = alienDeployer;
    }

    AlienDeployer alienDeployer;

    public void ShowRadarBlips() {
        GameLogicComponent.world.ClearRadarBlips();
        foreach (var virtualAlien in alienDeployer.hiddenAliens) {
            if (virtualAlien.radarPresence > 0.5f) {
                GameLogicComponent.world.CreateRadarBlip(virtualAlien.gridLocation);
            }
        }
    }

    public void ClearRadarBlips() {
        GameLogicComponent.world.ClearRadarBlips();
    }
}
