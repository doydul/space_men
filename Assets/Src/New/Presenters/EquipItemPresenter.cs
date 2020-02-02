using Data;

public class EquipItemPresenter : Presenter, IPresenter<EquipItemOutput> {
    
    public BlackFade blackFade;

    public static EquipItemPresenter instance { get; private set; }
    
    void Awake() {
        instance = this;
    }
    
    public void Present(EquipItemOutput input) {
        blackFade.BeginFade(() => {
            InventoryInitializer.OpenScene(new InventoryInitializer.Args {
                metaSoldierId = input.soldierId,
                armourName = input.armourName,
                weaponName = input.weaponName
            });
        });
    }
}

