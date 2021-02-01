using Data;
using Workers;

namespace Interactors {
    
    public class MoveSoldierInteractor : Interactor<MoveSoldierOutput> {

        [Dependency] GameState gameState;
        public ISoldierStore soldierStore { private get; set; }

        public void Interact(MoveSoldierInput input) {
            var output = new MoveSoldierOutput();
            var soldierMove = new SoldierMove(input.soldierIndex, input.targetPosition);
            soldierMove.Execute(gameState, metaGameState);
            
            var soldier = gameState.GetActor(input.soldierIndex) as SoldierActor;
            var armourStats = soldierStore.GetArmourStats(soldier.armourName);
            
            output.soldierIndex = input.soldierIndex;
            output.newPosition = input.targetPosition;
            output.newFacing = soldier.facing;
            output.movementType = soldier.moved <= armourStats.movement ? MovementType.Running : MovementType.Sprinting;
            output.traversedCells = soldierMove.traversedCells;
            output.newFogs = FogHandler.ApplyFog(gameState);
            output.damageInstances = soldierMove.damageInstances;
            
            presenter.Present(output);
        }
    }
}
