using Data;

public class OpenInventoryPresenter : Presenter, IPresenter<OpenInventoryOutput> {
    
    public static OpenInventoryPresenter instance { get; private set; }

    public BlackFade blackFade;
    
    void Awake() {
        instance = this;
    }
    
    public void Present(OpenInventoryOutput input) {
        blackFade.BeginFade(() => {
            InventoryInitializer.OpenScene(new InventoryInitializer.Args {
                metaSoldierId = input.metaSoldierId,
                armourName = input.armourName,
                weaponName = input.weaponName
            });
        });
    }
}

