using Interactors;
using Data;

public class MetaGameController : Controller {
    
    // public DoSomeActionInteractor doSomeActionInteractor { get; set; }
    
    public void DoSomeAction() {
        if (!disabled) {
            // doSomeActionInteractor.Interact(new DoSomeActionInput());
        }
    }
}
