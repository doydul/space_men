using Data;

public class FinishMissionPresenter : Presenter, IPresenter<FinishMissionOutput> {
  
    public static FinishMissionPresenter instance { get; private set; }
    
    void Awake() {
        instance = this;
    }
    
    public void Present(FinishMissionOutput input) {
        // TODO
    }
}

