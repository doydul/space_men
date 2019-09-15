namespace Interactors {
    
    public abstract class Interactor<T> {
        
        public IPresenter<T> presenter { protected get; set; }
        public Workers.GameState gameState { protected get; set; }
    }
}
