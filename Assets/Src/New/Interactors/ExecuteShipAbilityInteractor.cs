using Data;
using Workers;

namespace Interactors {
    
    public class ExecuteShipAbilityInteractor : Interactor<ExecuteShipAbilityOutput> {

        public void Interact(ExecuteShipAbilityInput input) {
            var output = new ExecuteShipAbilityOutput();
            if (!gameState.shipEnergy.full) {
                UnityEngine.Debug.Log("not enough energy");
            }
            gameState.shipEnergy.Drain();
            output.newShipEnergyLevel = gameState.shipEnergy.value;
            
            switch(input.abilityType) {
                case ShipAbilityType.TeleportAmmoIn:
                    if (!DoTeleportAmmoIn(input, ref output)) return;
                    break;

                case ShipAbilityType.TeleportSoldierIn:
                    if (!DoTeleportSoldierIn(input, ref output)) return;
                    break;
            }
            
            presenter.Present(output);
        }

        bool DoTeleportAmmoIn(ExecuteShipAbilityInput input, ref ExecuteShipAbilityOutput output) {
            return false;
        }

        bool DoTeleportSoldierIn(ExecuteShipAbilityInput input, ref ExecuteShipAbilityOutput output) {
            if (gameState.currentPhase != Data.GamePhase.Movement || metaGameState.metaSoldiers.squadCount >= 4) {
                return false;
            }
            var metaSoldier = metaGameState.metaSoldiers.Get(input.metaSoldierId);
            metaGameState.metaSoldiers.FillFirstEmptySquadSlot(input.metaSoldierId);
            var soldier = SoldierFromMetaSoldier(metaSoldier);
            soldier.position = input.targetPosition;
            soldier.facing = (Direction)UnityEngine.Random.Range(0, 4);
            gameState.AddActor(soldier);
            output.newSoldier = soldier.ToValueType();
            return true;
        }

        SoldierActor SoldierFromMetaSoldier(MetaSoldier metaSoldier) {
            return SoldierGenerator.Default()
                                   .WithArmour(metaSoldier.armour.name)
                                   .WithWeapon(metaSoldier.weapon.name)
                                   .WithMetaSoldierId(metaSoldier.uniqueId)
                                   .Build();
        }
    }
}
