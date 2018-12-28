public class MovementPhase : Phase {

    public MovementPhase(Map map, RadarBlipController radarBlipController) {
        this.map = map;
        this.radarBlipController = radarBlipController;
    }

    Map map;
    RadarBlipController radarBlipController;
    bool proceeded;

    public override bool finished { get {
        return proceeded;
    } }

    public override void Start() {
        foreach (var soldier in map.GetActors<Soldier>()) {
            soldier.StartMovementPhase();
        }
        radarBlipController.ShowRadarBlips();
    }

    public override DelayedAction Proceed() {
        var result = new DelayedAction();
        proceeded = true;
        result.Finish();
        return result;
    }
}
