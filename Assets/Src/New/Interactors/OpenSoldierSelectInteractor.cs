using System.Linq;
using Data;
using Workers;

namespace Interactors {
    
    public class OpenSoldierSelectInteractor : Interactor<OpenSoldierSelectOutput> {

        public void Interact(OpenSoldierSelectInput input) {
            var output = new OpenSoldierSelectOutput();
            
            var currentMetaSoldier = metaGameState.metaSoldiers.GetAtSquadIndex(input.squadPositionIndex);
            var squadIds = metaGameState.metaSoldiers.GetSquad()
                                                     .Where(metaSoldier => metaSoldier != null)
                                                     .Select(metaSoldier => metaSoldier.uniqueId);
            output.currentSoldier = new SoldierDisplayInfo {
                soldierId = currentMetaSoldier.uniqueId,
                weaponName = currentMetaSoldier.weapon.name,
                armourName = currentMetaSoldier.armour.name
            };
            output.selectableSoldiers = metaGameState.metaSoldiers.GetAll()
                                                                  .Where(metaSoldier => !squadIds.Contains(metaSoldier.uniqueId))
                                                                  .Select(metaSoldier => new SoldierDisplayInfo {
                                                                      soldierId = metaSoldier.uniqueId,
                                                                      weaponName = metaSoldier.weapon.name,
                                                                      armourName = metaSoldier.armour.name
                                                                  })
                                                                  .ToArray();
            
            presenter.Present(output);
        }
    }
}
