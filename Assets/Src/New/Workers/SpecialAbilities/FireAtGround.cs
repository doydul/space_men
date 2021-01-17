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
        public override SpecialAbilityType type => SpecialAbilityType.FireAtGround;
        public override Position[] possibleTargetSquares { get {
            var fireArc = factory.MakeObject<FireArc>(soldier.position, soldier.facing);
            var iterator = new CellIterator(soldier.position, cell => !cell.isFoggy);
            return iterator.Iterate(gameState.map)
                .Where(cell => fireArc.WithinArcAndLOS(cell.cell.position))
                .Select(cell => cell.cell.position)
                .ToArray();
        } }

        public override object Execute() {
            var output = new Output();
            var explosion = factory.MakeObject<Explosion>();
            explosion.CalculateFromSoldier(soldier.uniqueId, input.targetSquare);

            soldier.IncrementShotsFired();
            output.soldierIndex = soldier.uniqueId;
            output.weaponName = soldier.weaponName;
            output.shotsLeft = soldier.shotsRemaining;
            output.ammoLeft = soldier.ammoRemaining;
            output.maxAmmo = soldier.maxAmmo;
            output.explosion = new ExplosionData {
                squaresCovered = explosion.coveredTiles,
                damageInstances = explosion.damageInstances,
                fires = explosion.fires
            };
            return output;
        }

        public struct Input {
            public long soldierId;
            public Position targetSquare;
        }

        public struct Output {
            public long soldierIndex;
            public string weaponName;
            public int shotsLeft;
            public int ammoLeft;
            public int maxAmmo;
            public ExplosionData explosion;
        }
    }
}