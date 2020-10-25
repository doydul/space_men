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

        public override SpecialAbilityOutput Execute() {
            var result = new SpecialAbilityOutput();
            soldier.RefillAmmo();
            soldier.DisableShooting();
            var crate = soldier.cell.backgroundActor;
            crate.health.Damage(1);

            result.soldierIndex = soldier.uniqueId;
            result.maxAmmoCount = soldier.maxAmmo;
            result.newAmmoCount = soldier.ammoRemaining;
            result.remainingCrateSupplies = crate.health.current;

            if (crate.health.dead) {
                gameState.RemoveActor(crate.uniqueId);
            }            
            return result;
        }

        public struct Input {
            public long soldierId;
        }
    }
}