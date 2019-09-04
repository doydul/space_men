using UnityEngine;

public class RadarBlipController {
    
    public static RadarBlipController instance { get; private set; }
    
    public RadarBlipController(AlienDeployer alienDeployer) {
        this.alienDeployer = alienDeployer;
        instance = this;
    }

    AlienDeployer alienDeployer;

    public void ShowRadarBlips() {
        GameLogicComponent.world.ClearRadarBlips();
        foreach (var virtualAlien in alienDeployer.hiddenAliens) {
            var alienType = Resources.Load<AlienData>("Aliens/" + virtualAlien.alienType);
            if (virtualAlien.radarPresence < alienType.chanceOfCreatingRadarBlip) {
                GameLogicComponent.world.CreateRadarBlip(virtualAlien.gridLocation);
            }
        }
    }

    public void ClearRadarBlips() {
        GameLogicComponent.world.ClearRadarBlips();
    }
}
