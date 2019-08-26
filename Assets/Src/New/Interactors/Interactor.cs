namespace Interactors {
    
    public abstract class Interactor<T> {
        
        public IPresenter<T> presenter { protected get; set; }
    }
}
