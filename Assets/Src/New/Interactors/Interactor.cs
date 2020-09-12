namespace Interactors {
    
    public abstract class Interactor<T> {
        
        public IPresenter<T> presenter { protected get; set; }
        public Workers.MetaGameState metaGameState => Workers.MetaGameState.instance;

        [Dependency] protected Workers.GameState gameState;
    }
}
