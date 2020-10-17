using System.Linq;
using Data;

namespace Workers {
    public class FireAtGround : SpecialAbility {

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

        public FireAtGround(Input input) {
            this.input = input;
        }

        public override bool usable => soldier.hasBlastWeapon && soldier.CanShoot() && gameState.currentPhase == Data.GamePhase.Shooting;
        public override SpecialActionType type => SpecialActionType.FireAtGround;
        public override Position[] possibleTargetSquares { get {
            var fireArc = factory.MakeObject<FireArc>(soldier.position, soldier.facing);
            var iterator = new CellIterator(soldier.position, cell => !cell.isFoggy);
            return iterator.Iterate(gameState.map)
                .Where(cell => fireArc.WithinArcAndLOS(cell.cell.position))
                .Select(cell => cell.cell.position)
                .ToArray();
        } }

        public override SpecialAbilityOutput Execute() {
            var result = new SpecialAbilityOutput();
            var explosion = factory.MakeObject<Explosion>();
            explosion.CalculateFromSoldier(soldier.uniqueId, input.targetSquare);

            soldier.IncrementShotsFired();
            result.soldierIndex = soldier.uniqueId;
            result.weaponName = soldier.weaponName;
            result.shotsLeft = soldier.shotsRemaining;
            result.ammoLeft = soldier.ammoRemaining;
            result.maxAmmo = soldier.maxAmmo;
            result.damageInstances = explosion.damageInstances;
            result.blastCoverage = explosion.coveredTiles;
            return result;
        }

        public struct Input {
            public long soldierId;
            public Position targetSquare;
        }
    }
}