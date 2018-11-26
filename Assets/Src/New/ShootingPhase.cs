public class ShootingPhase : Phase {

    private const int SHOOTING_PHASE_ITERATIONS = 4;

    Map map;
    AlienMovementPhaseDirector alienMovementPhaseDirector;
    AlienDeployer alienDeployer;
    int iterations;

    public ShootingPhase(
        Map map,
        AlienMovementPhaseDirector alienMovementPhaseDirector,
        AlienDeployer alienDeployer
    ) {
        this.map = map;
        this.alienMovementPhaseDirector = alienMovementPhaseDirector;
        this.alienDeployer = alienDeployer;
    }

    public override bool finished { get {
        return iterations >= SHOOTING_PHASE_ITERATIONS;
    } }

    public override void Start() {
        foreach (var soldier in map.GetActors<Soldier>()) {
            soldier.StartShootingPhase();
        }
        GameEvents.Trigger("ShootingPhaseStart");
    }

    public override void Proceed() {
        iterations++;
        alienDeployer.Iterate();
        alienMovementPhaseDirector.MoveAliens(() => {
           if (!AnyPlayerActionsPossible() && !finished) Proceed();
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
