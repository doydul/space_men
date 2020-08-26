using Data;

public class SelectionMenuCancelPresenter : Presenter, IPresenter<SelectionMenuCancelOutput> {
  
    public static SelectionMenuCancelPresenter instance { get; private set; }

    public BlackFade blackFade;
    
    public SelectionMenuInitializer.Args args { private get; set; }

    void Awake() {
        instance = this;
    }
    
    public void Present(SelectionMenuCancelOutput input) {
        blackFade.BeginFade(() => {
            args.backAction();
        });
    }
}

