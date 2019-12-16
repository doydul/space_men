using Data;

public class CompleteSecondaryMissionPresenter : Presenter, IPresenter<CompleteSecondaryMissionOutput> {
  
    public static CompleteSecondaryMissionPresenter instance { get; private set; }
    
    void Awake() {
        instance = this;
    }
    
    public void Present(CompleteSecondaryMissionOutput input) {
        
    }
}

