public class ShootingPhase : Phase {

    private const int SHOOTING_PHASE_ITERATIONS = 3;

    public ShootingPhase(
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
    int iterations;

    public override bool finished { get {
        return iterations >= SHOOTING_PHASE_ITERATIONS;
    } }

    public override void Start() {
        foreach (var soldier in map.GetActors<Soldier>()) {
            soldier.StartShootingPhase();
        }
        radarBlipController.ClearRadarBlips();
        alienDeployer.Iterate();
    }

    public override DelayedAction Proceed() {
        var result = new DelayedAction();
        Proceed(result);
        return result;
    }

    void Proceed(DelayedAction previousResult) {
        if (finished) {
            previousResult.Finish();
            return;
        }
        iterations++;
        alienMovementPhaseDirector.MoveAliens().Then(() => {
            if (!AnyPlayerActionsPossible()) {
                Proceed(previousResult);
            }
        });
    }

    bool AnyPlayerActionsPossible() {
        foreach (var soldier in map.GetActors<Soldier>()) {
            var possibleActions = new PossibleSoldierActions(map, soldier);
            if (possibleActions.Any()) return true;
        }
        return false;
    }
}
