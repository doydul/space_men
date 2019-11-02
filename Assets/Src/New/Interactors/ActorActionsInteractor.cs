using Data;
using Workers;

namespace Interactors {
    
    public class ActorActionsInteractor : Interactor<ActorActionsOutput> {

        public void Interact(ActorActionsInput input) {
            var output = new ActorActionsOutput();
            
            // TODO
            
            presenter.Present(output);
        }
    }
}
