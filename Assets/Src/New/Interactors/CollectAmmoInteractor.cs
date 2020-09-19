using Data;
using Workers;

namespace Interactors {
    
    public class CollectAmmoInteractor : Interactor<CollectAmmoOutput> {

        [Dependency] GameState gameState;
        [Dependency] IInstantiator factory;

        public void Interact(CollectAmmoInput input) {
            var output = new CollectAmmoOutput();

            var soldier = gameState.GetActor(input.soldierIndex) as SoldierActor;
            var decorator = factory.MakeObject<SoldierDecorator>(soldier);
            var crate = decorator.cell.backgroundActor;

            if (decorator.ammoSpent <= 0 || !crate.isCrate) return;

            decorator.RefillAmmo();
            decorator.DisableShooting();
            crate.health.Damage(1);

            output.soldierIndex = soldier.uniqueId;
            output.maxAmmoCount = decorator.maxAmmo;
            output.newAmmoCount = decorator.ammoRemaining;
            output.remainingCrateSupplies = crate.health.current;

            if (crate.health.dead) {
                gameState.RemoveActor(crate.uniqueId);
            }
            
            presenter.Present(output);
        }
    }
}
