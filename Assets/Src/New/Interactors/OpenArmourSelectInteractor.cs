using System.Linq;

using Data;
using Workers;

namespace Interactors {
    
    public class OpenArmourSelectInteractor : Interactor<OpenArmourSelectOutput> {

        [Dependency] ISoldierStore soldierStore;

        public void Interact(OpenArmourSelectInput input) {
            var output = new OpenArmourSelectOutput();
            
            var metaSoldier = metaGameState.metaSoldiers.Get(input.metaSoldierId);
            output.inventoryArmour = metaGameState.metaItems
                                          .GetInventoryItems()
                                          .Where(item => item is MetaArmour)
                                          .Select(item => new OpenArmourSelectOutput.ArmourInfo {
                                              itemId = item.uniqueId,
                                              name = item.name,
                                              armourStats = soldierStore.GetArmourStats(item.name)
                                          }).ToArray();
            output.currentArmour = new OpenArmourSelectOutput.ArmourInfo {
                itemId = metaSoldier.armour.uniqueId,
                name = metaSoldier.armour.name
            };
            output.soldierId = metaSoldier.uniqueId;
            
            presenter.Present(output);
        }
    }
}
