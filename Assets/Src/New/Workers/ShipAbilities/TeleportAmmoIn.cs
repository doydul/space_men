using System.Linq;
using Data;

namespace Workers {

    public class TeleportAmmoIn : ShipAbility {

        [Dependency] GameState gameState;

        Input input;

        public TeleportAmmoIn(Input input) {
            this.input = input;
        }

        public override bool usable { get {
            return gameState.currentPhase == Data.GamePhase.Movement
                && gameState.shipEnergy.value >= 2;
        } }
        public override ShipAbilityType type => ShipAbilityType.TeleportAmmoIn;
        public override Position[] possibleTargetSquares { get {
            return gameState.map.GetAllCells()
                .Where(cell => !cell.isFoggy && !cell.isWall && !cell.hasActor)
                .Select(cell => cell.position)
                .ToArray();
        } }

        public override ShipAbilityOutput Execute() {
            var crateActor = new CrateActor {
                position = input.targetSquare,
                health = new Health(8)
            };
            gameState.AddActor(crateActor, true);
            gameState.shipEnergy.Change(-2);

            return new ShipAbilityOutput {
                newAmmoCrate = new Crate {
                    index = crateActor.uniqueId,
                    position = crateActor.position,
                    remainingUses = crateActor.health.current
                }
            };
        }

        public struct Input {
            public Position targetSquare;
        }
    }
}