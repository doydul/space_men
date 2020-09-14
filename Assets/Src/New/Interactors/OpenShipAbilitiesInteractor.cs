using System.Linq;
using Data;
using Workers;

namespace Interactors {
    
    public class OpenShipAbilitiesInteractor : Interactor<OpenShipAbilitiesOutput> {

        [Dependency] GameState gameState;
        [Dependency] IInstantiator factory;

        public void Interact(OpenShipAbilitiesInput input) {
            var output = new OpenShipAbilitiesOutput();

            var shipAbilities = new ShipAbility[] {
                factory.MakeObject<TeleportSoldierIn>(new TeleportSoldierIn.Input()),
                factory.MakeObject<TeleportAmmoIn>(new TeleportAmmoIn.Input())
            };

            output.abilities = shipAbilities.Select(shipAbility => new ShipAbilityInfo {
                type = shipAbility.type,
                usable = shipAbility.usable
            }).ToArray();
            
            presenter.Present(output);
        }
    }
}
