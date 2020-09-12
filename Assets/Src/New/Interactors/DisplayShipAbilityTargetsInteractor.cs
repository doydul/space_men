using Data;
using Workers;
using System.Linq;

namespace Interactors {
    
    public class DisplayShipAbilityTargetsInteractor : Interactor<DisplayShipAbilityTargetsOutput> {

        public void Interact(DisplayShipAbilityTargetsInput input) {
            var output = new DisplayShipAbilityTargetsOutput();
            if (!gameState.shipEnergy.full) return;
            
            switch(input.abilityType) {
                case ShipAbilityType.TeleportAmmoIn:
                    GenerateTelePortAmmoInTargets(ref output);
                    break;

                case ShipAbilityType.TeleportSoldierIn:
                    GenerateTelePortSoldierInTargets(ref output);
                    break;
            }
            
            presenter.Present(output);
        }

        void GenerateTelePortAmmoInTargets(ref DisplayShipAbilityTargetsOutput output) {
            output.possibleActions = gameState.map.GetAllCells()
                .Where(cell => !cell.isFoggy && !cell.isWall && !cell.hasActor)
                .Select(cell => new ShipAction { type = ShipAbilityType.TeleportAmmoIn, target = cell.position })
                .ToArray();
        }

        void GenerateTelePortSoldierInTargets(ref DisplayShipAbilityTargetsOutput output) {
            output.possibleActions = gameState.map.GetAllCells()
                .Where(cell => !cell.isFoggy && !cell.isWall && !cell.hasActor)
                .Select(cell => new ShipAction { type = ShipAbilityType.TeleportSoldierIn, target = cell.position })
                .ToArray();
            output.possibleTargetMetaSoldiers = metaGameState.metaSoldiers.GetIdle()
                .Select(metaSoldier => ConvertMetaSoldier(metaSoldier))
                .ToArray();
        }

        SoldierDisplayInfo ConvertMetaSoldier(MetaSoldier metaSoldier) {
            return new SoldierDisplayInfo {
                soldierId = metaSoldier.uniqueId,
                name = metaSoldier.name,
                weaponName = metaSoldier.weapon.name,
                armourName = metaSoldier.armour.name
            };
        }
    }
}
