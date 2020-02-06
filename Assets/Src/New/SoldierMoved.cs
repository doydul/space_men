public class SoldierMoved : IGameEvent {

    public SoldierMoved(FogController fogController) {
        this.fogController = fogController;
    }

    FogController fogController;

    public void Invoke() {
        // fogController.Recalculate();
    }
}
