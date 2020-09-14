using System.Linq;
using Data;

namespace Workers {

    public class TeleportSoldierIn : ShipAbility {

        [Dependency] GameState gameState;
        [Dependency] MetaGameState metaGameState;

        Input input;

        public TeleportSoldierIn(Input input) {
            this.input = input;
        }

        public override bool usable { get {
            return gameState.currentPhase == Data.GamePhase.Movement
                          && gameState.shipEnergy.full
                          && metaGameState.metaSoldiers.squadCount < 4
                          && metaGameState.metaSoldiers.GetIdle().Any();
        } }
        public override ShipAbilityType type => ShipAbilityType.TeleportSoldierIn;
        public override Position[] possibleTargetSquares { get {
            return gameState.map.GetAllCells()
                .Where(cell => !cell.isFoggy && !cell.isWall && !cell.hasActor)
                .Select(cell => cell.position)
                .ToArray();
        } }
        public override MetaSoldier[] possibleTargetMetaSoldiers { get {
            return metaGameState.metaSoldiers.GetIdle().ToArray();
        } }

        public override ShipAbilityOutput Execute() {
            var metaSoldier = metaGameState.metaSoldiers.Get(input.metaSoldierId);
            metaGameState.metaSoldiers.FillFirstEmptySquadSlot(input.metaSoldierId);
            var soldier = SoldierFromMetaSoldier(metaSoldier);
            soldier.position = input.targetSquare;
            soldier.facing = (Direction)UnityEngine.Random.Range(0, 4);
            gameState.AddActor(soldier);
            gameState.shipEnergy.Drain();

            return new ShipAbilityOutput {
                newSoldier = soldier.ToValueType()
            };
        }

        bool DoTeleportSoldierIn(ExecuteShipAbilityInput input, ref ExecuteShipAbilityOutput output) {
            
            return true;
        }

        SoldierActor SoldierFromMetaSoldier(MetaSoldier metaSoldier) {
            return SoldierGenerator.Default()
                                   .WithArmour(metaSoldier.armour.name)
                                   .WithWeapon(metaSoldier.weapon.name)
                                   .WithMetaSoldierId(metaSoldier.uniqueId)
                                   .Build();
        }

        public struct Input {
            public long metaSoldierId;
            public Position targetSquare;
        }
    }
}