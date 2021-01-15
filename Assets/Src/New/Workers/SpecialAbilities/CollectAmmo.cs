using System.Linq;
using Data;

namespace Workers {
    public class CollectAmmo : SpecialAbility {

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

        public CollectAmmo(Input input) {
            this.input = input;
        }

        public override bool usable => soldier.cell.backgroundActor.isCrate && soldier.ammoSpent > 0;
        public override SpecialAbilityType type => SpecialAbilityType.CollectAmmo;
        public override bool executeImmediately => true;

        public override object Execute() {
            var output = new Output();
            soldier.RefillAmmo();
            soldier.DisableShooting();
            var crate = soldier.cell.backgroundActor;
            crate.health.Damage(1);

            output.soldierIndex = soldier.uniqueId;
            output.maxAmmoCount = soldier.maxAmmo;
            output.newAmmoCount = soldier.ammoRemaining;
            output.remainingCrateSupplies = crate.health.current;

            if (crate.health.dead) {
                gameState.RemoveActor(crate.uniqueId);
            }            
            return output;
        }

        public struct Input {
            public long soldierId;
        }

        public struct Output {
            public long soldierIndex;
            public int maxAmmoCount;
            public int newAmmoCount;
            public int remainingCrateSupplies;
        }
    }
}