using System.Linq;
using Data;
using Workers;

namespace Interactors {
    
    public class ExecuteShipAbilityInteractor : Interactor<ExecuteShipAbilityOutput> {

        [Dependency] GameState gameState;
        [Dependency] IInstantiator factory;

        public void Interact(ExecuteShipAbilityInput input) {
            var output = new ExecuteShipAbilityOutput();

            var shipAbilities = new ShipAbility[] {
                factory.MakeObject<TeleportSoldierIn>(new TeleportSoldierIn.Input {
                    metaSoldierId = input.metaSoldierId,
                    targetSquare = input.targetPosition
                }),
                factory.MakeObject<TeleportAmmoIn>(new TeleportAmmoIn.Input())
            };

            var ability = shipAbilities.First(shipAbility => shipAbility.type == input.abilityType);
            if (!ability.usable) return;
            
            output.shipAbilityOutput = ability.Execute();
            output.newShipEnergyLevel = gameState.shipEnergy.value;
            
            presenter.Present(output);
        }
    }
}
