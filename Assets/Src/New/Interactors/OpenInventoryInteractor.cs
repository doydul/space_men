using Data;
using Workers;

namespace Interactors {
    
    public class OpenInventoryInteractor : Interactor<OpenInventoryOutput> {

        public void Interact(OpenInventoryInput input) {
            var output = new OpenInventoryOutput();
            
            var metaSoldier = metaGameState.metaSoldiers.Get(input.metaSoldierId);
            output.metaSoldierId = input.metaSoldierId;
            output.armourName = metaSoldier.armour.name;
            output.weaponName = metaSoldier.weapon.name;
            
            presenter.Present(output);
        }
    }
}
