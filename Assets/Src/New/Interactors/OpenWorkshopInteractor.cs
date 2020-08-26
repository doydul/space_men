using System.Linq;
using Data;
using Workers;

namespace Interactors {
    
    public class OpenWorkshopInteractor : Interactor<OpenWorkshopOutput> {

        public void Interact(OpenWorkshopInput input) {
            var output = new OpenWorkshopOutput();
            
            var itemsList = new WorkshopItemList(metaGameState);
            output.state = itemsList.GetList();
            
            presenter.Present(output);
        }
    }
}
