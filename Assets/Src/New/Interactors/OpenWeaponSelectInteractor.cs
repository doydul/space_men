using System.Linq;
using Data;
using Workers;

namespace Interactors {
    
    public class OpenWeaponSelectInteractor : Interactor<OpenWeaponSelectOutput> {

        [Dependency] ISoldierStore soldierStore;

        public void Interact(OpenWeaponSelectInput input) {
            var output = new OpenWeaponSelectOutput();
            
            var metaSoldier = metaGameState.metaSoldiers.Get(input.metaSoldierId);
            output.inventoryWeapons = metaGameState.metaItems
                                          .GetInventoryItems()
                                          .Where(item => item is MetaWeapon)
                                          .Select(item => new OpenWeaponSelectOutput.WeaponInfo {
                                              itemId = item.uniqueId,
                                              name = item.name,
                                              weaponStats = soldierStore.GetWeaponStats(item.name)
                                          }).ToArray();
            output.currentWeapon = new OpenWeaponSelectOutput.WeaponInfo {
                itemId = metaSoldier.weapon.uniqueId,
                name = metaSoldier.weapon.name
            };
            
            presenter.Present(output);
        }
    }
}
