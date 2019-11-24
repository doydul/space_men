using Data;
using Workers;

namespace Interactors {
    
    public class FinishMissionInteractor : Interactor<FinishMissionOutput> {

        public void Interact(FinishMissionInput input) {
            var output = new FinishMissionOutput();
            
            
            
            presenter.Present(output);
        }
    }
}
