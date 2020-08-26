using Data;
using Workers;

namespace Interactors {
    
    public class SelectionMenuCancelInteractor : Interactor<SelectionMenuCancelOutput> {

        public void Interact(SelectionMenuCancelInput input) {
            var output = new SelectionMenuCancelOutput();
            presenter.Present(output);
        }
    }
}
