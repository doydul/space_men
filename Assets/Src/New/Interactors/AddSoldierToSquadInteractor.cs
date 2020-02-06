using Data;
using Workers;

namespace Interactors {
    
    public class AddSoldierToSquadInteractor : Interactor<AddSoldierToSquadOutput> {

        public void Interact(AddSoldierToSquadInput input) {
            var output = new AddSoldierToSquadOutput();
            metaGameState.metaSoldiers.UpdateSquadRoster(input.soldierId, input.index);
            presenter.Present(output);
        }
    }
}
