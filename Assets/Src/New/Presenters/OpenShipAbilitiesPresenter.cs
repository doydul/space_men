using Data;

public class OpenShipAbilitiesPresenter : Presenter, IPresenter<OpenShipAbilitiesOutput> {
  
    public static OpenShipAbilitiesPresenter instance { get; private set; }

    public ShipAbilitiesPanel abilitiesPanel;
    
    void Awake() {
        instance = this;
    }
    
    public void Present(OpenShipAbilitiesOutput input) {
        abilitiesPanel.Open(input.abilities);
    }
}

