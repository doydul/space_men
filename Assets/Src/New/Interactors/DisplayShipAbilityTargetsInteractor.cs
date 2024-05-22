using Data;
using Workers;
using System.Linq;

namespace Interactors {
    
    public class DisplayShipAbilityTargetsInteractor : Interactor<DisplayShipAbilityTargetsOutput> {

        [Dependency] GameState gameState;
        [Dependency] IInstantiator factory;

        public void Interact(DisplayShipAbilityTargetsInput input) {
            var output = new DisplayShipAbilityTargetsOutput();
            
            var shipAbilities = new ShipAbility[] {
                factory.MakeObject<TeleportSoldierIn>(new TeleportSoldierIn.Input()),
                factory.MakeObject<TeleportAmmoIn>(new TeleportAmmoIn.Input())
            };
            
            var ability = shipAbilities.First(shipAbility => shipAbility.type == input.abilityType);

            if (!ability.usable) return;

            output.possibleActions = ability.possibleTargetSquares
                .Select(pos => new ShipAction { type = ability.type, target = pos })
                .ToArray();
            output.possibleTargetMetaSoldiers = ability.possibleTargetMetaSoldiers
                .Select(metaSoldier => ConvertMetaSoldier(metaSoldier))
                .ToArray();
            
            presenter.Present(output);
        }

        SoldierDisplayInfo ConvertMetaSoldier(MetaSoldier metaSoldier) {
            return new SoldierDisplayInfo {
                name = metaSoldier.name,
                weaponName = metaSoldier.weapon.name,
                armourName = metaSoldier.armour.name
            };
        }
    }
}
