using Data;
using Workers;

namespace Interactors {
    
    public class HireSolderInteractor : Interactor<HireSolderOutput> {

        public ISoldierNameGenerator nameGenerator { private get; set; }

        public void Interact(HireSolderInput input) {
        //     var output = new HireSolderOutput();

        //     int soldierCost = 100;
            
        //     if (metaGameState.credits.ContainsAtleast(soldierCost)) {
        //         metaGameState.credits.Deduct(soldierCost);
        //         var weapon = new MetaWeapon { name = "Assault Rifle" };
        //         var armour = new MetaArmour { name = "Basic" };
        //         metaGameState.metaItems.Add(weapon);
        //         metaGameState.metaItems.Add(armour);
        //         metaGameState.metaSoldiers.Add(new MetaSoldier {
        //             name = nameGenerator.Generate(),
        //             weapon = weapon,
        //             armour = armour
        //         });
        //         output.success = true;
        //     } else {
        //         output.success = false;
        //     }
        //     output.newCreditBalance = metaGameState.credits.value;
            
        //     presenter.Present(output);
        }
    }
}
