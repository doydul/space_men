using Data;

public class AddSoldierToSquadPresenter : Presenter, IPresenter<AddSoldierToSquadOutput> {
  
    public static AddSoldierToSquadPresenter instance { get; private set; }
    
    public BlackFade blackFade;
    
    [Dependency] SelectionMenuInitializer.Args args;

    void Awake() {
        instance = this;
    }
    
    public void Present(AddSoldierToSquadOutput input) {
        blackFade.BeginFade(() => {
            args.backAction();
        });
    }
}

