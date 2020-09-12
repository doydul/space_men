using System.Linq;
using Data;
using Workers;

namespace Interactors {
    
    public class OpenShipAbilitiesInteractor : Interactor<OpenShipAbilitiesOutput> {

        public void Interact(OpenShipAbilitiesInput input) {
            var output = new OpenShipAbilitiesOutput();
            
            output.abilities = new ShipAbilityInfo[] {
                new ShipAbilityInfo {
                    type = ShipAbilityType.TeleportSoldierIn,
                    usable = gameState.currentPhase == Data.GamePhase.Movement
                          && gameState.shipEnergy.full
                          && metaGameState.metaSoldiers.GetIdle().Any()
                },
                new ShipAbilityInfo {
                    type = ShipAbilityType.TeleportAmmoIn,
                    usable = gameState.shipEnergy.full
                }
            };
            
            presenter.Present(output);
        }
    }
}
