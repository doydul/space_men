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
                          && gameState.shipEnergy.full;
        } }
        public override ShipAbilityType type => ShipAbilityType.TeleportAmmoIn;

        public override ShipAbilityOutput Execute() {
            return new ShipAbilityOutput {

            };
        }

        public struct Input {
            public Position targetSquare;
        }
    }
}