using Data;

public class SelectionMenuCancelPresenter : Presenter, IPresenter<SelectionMenuCancelOutput> {
  
    public static SelectionMenuCancelPresenter instance { get; private set; }

    public BlackFade blackFade;
    
    [Dependency] SelectionMenuInitializer.Args args;

    void Awake() {
        instance = this;
    }
    
    public void Present(SelectionMenuCancelOutput input) {
        blackFade.BeginFade(() => {
            args.backAction();
        });
    }
}

