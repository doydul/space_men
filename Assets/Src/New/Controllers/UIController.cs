using Interactors;
using Data;

public class UIController : Controller {
    
    public ProgressGamePhaseInteractor progressGamePhaseInteractor { get; set; }
    
    public void ProgressGamePhase() {
        if (!disabled) progressGamePhaseInteractor.Interact(new ProgressGamePhaseInput());
    }
}
