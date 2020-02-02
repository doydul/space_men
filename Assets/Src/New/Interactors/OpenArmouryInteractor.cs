using System.Linq;

using Data;
using Workers;

namespace Interactors {
    
    public class OpenArmouryInteractor : Interactor<OpenArmouryOutput> {

        public void Interact(OpenArmouryInput input) {
            var output = new OpenArmouryOutput();
            
            output.squadSoldiers = metaGameState.metaSoldiers.GetSquad().Select(metaSoldier => metaSoldier == null ? null : new SoldierDisplayInfo {
                soldierId = metaSoldier.uniqueId,
                weaponName = metaSoldier.weapon.name,
                armourName = metaSoldier.armour.name
            }).ToArray();
            output.credits = metaGameState.credits.value;
            
            presenter.Present(output);
        }
    }
}
