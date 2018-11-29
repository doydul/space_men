public class FogChanged : IGameEvent {

    public FogChanged(AlienDeployer alienDeployer, RadarBlipController radarBlipController) {
        this.alienDeployer = alienDeployer;
        this.radarBlipController = radarBlipController;
    }

    AlienDeployer alienDeployer;
    RadarBlipController radarBlipController;

    public void Invoke() {
        alienDeployer.SpawnRevealedAliens();
        radarBlipController.ShowRadarBlips();
    }
}
