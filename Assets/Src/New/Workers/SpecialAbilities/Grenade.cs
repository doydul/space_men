using System.Linq;
using Data;

namespace Workers {
    public class Grenade : SpecialAbility {

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

        public Grenade(Input input) {
            this.input = input;
        }

        public override bool usable { get {
            return soldier.GetData<GrenadeAmmo>().spentAmmo < 1 &&
                soldier.weaponName == "Assault Rifle II" &&
                gameState.currentPhase == Data.GamePhase.Shooting;
        } }
        public override SpecialAbilityType type => SpecialAbilityType.Grenade;
        public override Position[] possibleTargetSquares { get {
            var fireArc = factory.MakeObject<FireArc>(soldier.position, soldier.facing);
            var iterator = new CellIterator(soldier.position, cell => !cell.isFoggy);
            return iterator.Iterate(gameState.map)
                .Where(node => fireArc.WithinArcAndLOS(node.cell.position))
                .Where(node => node.distanceFromStart <= MAX_THROW_DISTANCE)
                .Select(node => node.cell.position)
                .ToArray();
        } }

        public override object Execute() {
            var output = new Output();
            var explosion = factory.MakeObject<Explosion>();
            explosion.Calculate(new Explosion.Config {
                soldierId = soldier.uniqueId,
                target = input.targetSquare,
                accuracy = 60,
                blastSize = 5,
                minDamage = 1,
                maxDamage = 8,
                armourPen = 15
            });
            soldier.SetData(new GrenadeAmmo { spentAmmo = 1 });

            output.soldierIndex = soldier.uniqueId;
            output.explosion = new ExplosionData {
                squaresCovered = explosion.coveredTiles,
                damageInstances = explosion.damageInstances
            };
            return output;
        }

        public struct Input {
            public long soldierId;
            public Position targetSquare;
        }

        public struct Output {
            public long soldierIndex;
            public ExplosionData explosion;
        }
    }
}