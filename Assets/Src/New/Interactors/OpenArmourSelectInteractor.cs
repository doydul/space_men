using Data;
using Workers;

namespace Interactors {
    
    public class OpenArmourSelectInteractor : Interactor<OpenArmourSelectOutput> {

        public void Interact(OpenArmourSelectInput input) {
            var output = new OpenArmourSelectOutput();
            
            // TODO
            
            presenter.Present(output);
        }
    }
}
