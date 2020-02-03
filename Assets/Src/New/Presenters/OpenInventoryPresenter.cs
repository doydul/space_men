using Data;

public class OpenInventoryPresenter : Presenter, IPresenter<OpenInventoryOutput> {
    
    public static OpenInventoryPresenter instance { get; private set; }

    void Awake() {
        instance = this;
    }

    public Icon weaponIcon;
    public Icon armourIcon;
    
    public void Present(OpenInventoryOutput input) {
        weaponIcon.Init(Weapon.Get(input.weaponName));
        armourIcon.Init(Armour.Get(input.armourName));
    }
}

