using Data;

public class ActorActionsPresenter : Presenter, IPresenter<ActorActionsOutput> {
  
    public static ActorActionsPresenter instance { get; private set; }
    
    void Awake() {
        instance = this;
    }
    
    public void Present(ActorActionsOutput input) {
        // TODO
    }
}

