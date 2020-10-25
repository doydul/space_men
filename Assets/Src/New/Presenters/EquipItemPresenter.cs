using Data;

public class EquipItemPresenter : Presenter, IPresenter<EquipItemOutput> {
    
    public static EquipItemPresenter instance { get; private set; }

    public BlackFade blackFade;
    
    [Dependency] SelectionMenuInitializer.Args args;

    void Awake() {
        instance = this;
    }
    
    public void Present(EquipItemOutput input) {
        blackFade.BeginFade(() => {
            args.backAction();
        });
    }
}

