using System.Linq;
using Data;

namespace Workers {
    public class StunShot : SpecialAbility {

        const int MAX_THROW_DISTANCE = 6;

        [Dependency] GameState gameState;
        [Dependency] IInstantiator factory;

        Input input;
        SoldierDecorator _soldier;
        SoldierDecorator soldier { get {
            if (_soldier == null) {
                _soldier = factory.MakeObject<SoldierDecorator>(gameState.GetActor(input.soldierId) as SoldierActor);
            }
            return _soldier;
        } }

        public StunShot(Input input) {
            this.input = input;
        }

        public override bool usable { get {
            return soldier.GetData<StunShotAmmo>().spentAmmo < 1 &&
                soldier.weaponName == "Pulse Laser" &&
                gameState.currentPhase == Data.GamePhase.Shooting;
        } }
        public override SpecialAbilityType type => SpecialAbilityType.StunShot;
        public override Position[] possibleTargetSquares { get {
            var fireArc = factory.MakeObject<FireArc>(soldier.position, soldier.facing);
            var iterator = new CellIterator(soldier.position, cell => !cell.isFoggy);
            return iterator.Iterate(gameState.map)
                .Where(node => fireArc.WithinArcAndLOS(node.cell.position))
                .Where(node => node.cell.actor.isAlien)
                .Select(node => node.cell.position)
                .ToArray();
        } }

        public override object Execute() {
            var output = new Output();

            soldier.SetData(new StunShotAmmo { spentAmmo = 1 });
            var targetAlien = gameState.map.GetCell(input.targetSquare.x, input.targetSquare.y).actor;
            targetAlien.SetData(new StunStatus { isStunned = true });

            output.soldierIndex = soldier.uniqueId;
            output.targetIndex = targetAlien.uniqueId;

            return output;
        }

        public struct Input {
            public long soldierId;
            public Position targetSquare;
        }

        public struct Output {
            public long soldierIndex;
            public long targetIndex;
        }
    }
}