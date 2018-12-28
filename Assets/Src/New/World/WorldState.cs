public class WorldState {

    public static WorldState instance { get {
        if (_instance == null) _instance = new WorldState();
        return _instance;
    } }

    static WorldState _instance;

    public bool animationInProgress;

    public void StartOfAnimation() {
        animationInProgress = true;
    }

    public void EndOfAnimation() {
        animationInProgress = false;
    }
}
