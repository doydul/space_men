using Interactors;

public class UIController : Controller {
    
    public InspectSelectedItemInteractor inspectItemIntractor { private get; set; }
    
    public void InspectSoldier(int soldierIndex) {
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
}
