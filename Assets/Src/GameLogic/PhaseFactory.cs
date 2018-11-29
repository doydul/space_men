public class PhaseFactory : IPhaseFactory {

    public PhaseFactory(
        Map map,
        AlienMovementPhaseDirector alienMovementPhaseDirector,
        AlienDeployer alienDeployer,
        RadarBlipController radarBlipController
    ) {
        this.map = map;
        this.alienMovementPhaseDirector = alienMovementPhaseDirector;
        this.alienDeployer = alienDeployer;
        this.radarBlipController = radarBlipController;
    }

    Map map;
    AlienMovementPhaseDirector alienMovementPhaseDirector;
    AlienDeployer alienDeployer;
    RadarBlipController radarBlipController;

    public Phase MakeMovementPhase() {
        return new MovementPhase(
            map,
            radarBlipController
        );
    }

    public Phase MakeShootingPhase() {
        return new ShootingPhase(
            map,
            alienMovementPhaseDirector,
            alienDeployer,
            radarBlipController
        );
    }
}
