namespace Interactors {
    
    public abstract class Interactor<T> {
        
        [Dependency] protected IPresenter<T> presenter;
        public Workers.MetaGameState metaGameState => Workers.MetaGameState.instance;
    }
}
