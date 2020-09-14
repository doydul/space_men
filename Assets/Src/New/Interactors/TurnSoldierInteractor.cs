using Data;
using Workers;

namespace Interactors {
    
    public class TurnSoldierInteractor : Interactor<TurnSoldierOutput> {

        [Dependency] GameState gameState;

        public void Interact(TurnSoldierInput input) {
            var output = new TurnSoldierOutput {
                index = input.index,
                newFacing = input.newFacing
            };
            
            var soldier = gameState.GetActor(input.index) as SoldierActor;
            soldier.facing = input.newFacing;
            
            presenter.Present(output);
        }
    }
}
