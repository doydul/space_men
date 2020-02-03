using Data;

public class EquipItemPresenter : Presenter, IPresenter<EquipItemOutput> {
    
    public static EquipItemPresenter instance { get; private set; }

    public BlackFade blackFade;
    
    public SelectionMenuInitializer.Args args { private get; set; }

    void Awake() {
        instance = this;
    }
    
    public void Present(EquipItemOutput input) {
        blackFade.BeginFade(() => {
            args.backAction();
        });
    }
}

