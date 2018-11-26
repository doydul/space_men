public class MovementPhase : Phase {

    Map map;
    bool proceeded;

    public MovementPhase(Map map) {
        this.map = map;
    }

    public override bool finished { get {
        return proceeded;
    } }

    public override void Start() {
        foreach (var soldier in map.GetActors<Soldier>()) {
            soldier.StartMovementPhase();
        }
        GameEvents.Trigger("MovementPhaseStart");
    }

    public override void Proceed() {
        proceeded = true;
    }
}
