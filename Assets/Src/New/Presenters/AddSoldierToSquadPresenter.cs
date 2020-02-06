using Data;

public class AddSoldierToSquadPresenter : Presenter, IPresenter<AddSoldierToSquadOutput> {
  
    public static AddSoldierToSquadPresenter instance { get; private set; }
    
    public BlackFade blackFade;
    
    public SelectionMenuInitializer.Args args { private get; set; }

    void Awake() {
        instance = this;
    }
    
    public void Present(AddSoldierToSquadOutput input) {
        blackFade.BeginFade(() => {
            args.backAction();
        });
    }
}

