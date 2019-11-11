using Interactors;
using Data;

public class UIController : Controller {
    
    public InfoPanel infoPanel;
    public UIData uiData;

    public ProgressGamePhaseInteractor progressGamePhaseInteractor { get; set; }
    
    public void ProgressGamePhase() {
        if (!disabled) progressGamePhaseInteractor.Interact(new ProgressGamePhaseInput());
    }
    
    public void ShowSelectedActorInfo() {
        infoPanel.Display(uiData.selectedActor);
    }

    public void CloseInfoPanel() {
        infoPanel.Close();
    }
}
