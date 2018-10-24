using UnityEngine;

public class RadarBlipController : MonoBehaviour {

    public AlienDeployer alienDeployer;

    void Start() {
        GameEvents.On("FogChanged", ShowRadarBlips);
        GameEvents.On("MovementPhaseStart", ShowRadarBlips);
        GameEvents.On("ShootingPhaseStart", GameLogicComponent.world.ClearRadarBlips);
    }

    void ShowRadarBlips() {
        GameLogicComponent.world.ClearRadarBlips();
        foreach (var virtualAlien in alienDeployer.hiddenAliens) {
            if (virtualAlien.radarPresence > 0.5f) {
                GameLogicComponent.world.CreateRadarBlip(virtualAlien.gridLocation);
            }
        }
    }
}
