using Data;
using Workers;

namespace Interactors {
    
    public class MoveSoldierInteractor : Interactor<MoveSoldierOutput> {

        public ISoldierStore soldierStore { private get; set; }

        public void Interact(MoveSoldierInput input) {
            var output = new MoveSoldierOutput();
            var soldierMove = new SoldierMove(input.soldierIndex, input.targetPosition);
            soldierMove.Execute(gameState);
            
            var soldier = gameState.GetActor(input.soldierIndex) as SoldierActor;
            var armourStats = soldierStore.GetArmourStats(soldier.armourName);
            
            output.soldierIndex = input.soldierIndex;
            output.newPosition = input.targetPosition;
            output.newFacing = soldier.facing;
            output.movementType = soldier.moved <= armourStats.movement ? MovementType.Running : MovementType.Sprinting;
            output.traversedCells = soldierMove.traversedCells;
            
            presenter.Present(output);
        }
    }
}
