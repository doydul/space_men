using Interactors;
using Data;

public class UIController : Controller {
    
    public InspectSelectedItemInteractor inspectItemIntractor { private get; set; }
    public ProgressGamePhaseInteractor progressGamePhaseInteractor { private get; set; }
    
    public void InspectSoldier(long soldierIndex) {
        var input = new InspectSelectedItemDataObject {
            isBeingOpened = true,
            isSoldier = true,
            soldierIndex = soldierIndex
        };
        inspectItemIntractor.Interact(input);
    }
    
    public void CloseInfoPanel() {
        inspectItemIntractor.Interact(new InspectSelectedItemDataObject());
    }
    
    public void ProgressGamePhase() {
        progressGamePhaseInteractor.Interact(new ProgressGamePhaseInput());
    }
}
