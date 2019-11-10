using Interactors;
using Data;

public class UIController : Controller {
    
    public InspectSelectedItemInteractor inspectItemIntractor { get; set; }
    public ProgressGamePhaseInteractor progressGamePhaseInteractor { get; set; }
    
    public void InspectSoldier(long soldierIndex) {
        var input = new InspectSelectedItemDataObject {
            isBeingOpened = true,
            isSoldier = true,
            soldierIndex = soldierIndex
        };
        inspectItemIntractor.Interact(input);
    }
    
    public void CloseInfoPanel() {
        if (!disabled) inspectItemIntractor.Interact(new InspectSelectedItemDataObject());
    }
    
    public void ProgressGamePhase() {
        if (!disabled) progressGamePhaseInteractor.Interact(new ProgressGamePhaseInput());
    }
}
