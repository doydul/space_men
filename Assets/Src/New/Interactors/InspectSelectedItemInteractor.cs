using Workers;

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
                var soldierData = Storage.instance.GetSoldier(input.soldierIndex);
                result.infoText = "health: " + soldierData.health + "/" + soldierData.maxHealth + "\n" +
                                  "armour: " + soldierData.armourName + "\n"+
                                  "weapon: " + soldierData.weaponName + "\n"+
                                  "exp: " + soldierData.exp;
            } else {
                result.infoText = "This is an alien";
            }
            presenter.Present(result);
        }
    }
}
