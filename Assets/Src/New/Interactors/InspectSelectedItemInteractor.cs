using Workers;
using Data;

namespace Interactors {
    
    public class InspectSelectedItemInteractor : Interactor<SelectedItemInfoPresenterInputData> {
        
        public void Interact(InspectSelectedItemDataObject input) {
            var result = new SelectedItemInfoPresenterInputData();
            if (!input.isBeingOpened) {
                presenter.Present(result);
                return;
            }
            result.showInfoPanel = true;
            if (input.isSoldier) {
                var soldierData = gameState.GetActor(input.soldierIndex) as SoldierActor;
                result.infoText = "health: " + soldierData.health.current + "/" + soldierData.health.max + "\n" +
                                  "armour: " + soldierData.armourName.ToString() + "\n"+
                                  "exp: " + soldierData.exp;
            } else {
                result.infoText = "This is an alien";
            }
            presenter.Present(result);
        }
    }
}
