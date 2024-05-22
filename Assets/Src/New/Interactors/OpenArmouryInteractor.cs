using System.Linq;

using Data;
using Workers;

namespace Interactors {
    
    public class OpenArmouryInteractor : Interactor<OpenArmouryOutput> {

        public void Interact(OpenArmouryInput input) {
            var output = new OpenArmouryOutput();
            
            var list = metaGameState.metaSoldiers.GetSquad().Select(metaSoldier => metaSoldier == null ? null : new SoldierDisplayInfo {
                weaponName = metaSoldier.weapon.name,
                armourName = metaSoldier.armour.name
            });
            output.squadSoldiers = list.ToArray();
            
            output.credits = metaGameState.credits.value;
            
            presenter.Present(output);
        }
    }
}
