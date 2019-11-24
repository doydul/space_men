using Data;
using Workers;

namespace Interactors {
    
    public class MoveSoldierInteractor : Interactor<MoveSoldierOutput> {

        public void Interact(MoveSoldierInput input) {
            var output = new MoveSoldierOutput();
            var soldierMove = new SoldierMove(input.soldierIndex, input.targetPosition);
            soldierMove.Execute(gameState);
            
            var soldier = gameState.GetActor(input.soldierIndex) as SoldierActor;
            
            output.soldierIndex = input.soldierIndex;
            output.newPosition = input.targetPosition;
            output.newFacing = soldier.facing;
            output.movementType = soldier.moved <= soldier.baseMovement ? MovementType.Running : MovementType.Sprinting;
            output.traversedCells = soldierMove.traversedCells;
            
            presenter.Present(output);
        }
    }
}
