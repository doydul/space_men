using Data;
using Workers;

namespace Interactors {
    
    public class EquipItemInteractor : Interactor<EquipItemOutput> {

        public void Interact(EquipItemInput input) {
            // var output = new EquipItemOutput();
            
            // var items = metaGameState.metaItems;
            // var metaSoldier = metaGameState.metaSoldiers.Get(input.soldierId);
            // var item = items.Get(input.itemId);
            // var currentItem = items.Get(item is MetaWeapon ? metaSoldier.weapon.uniqueId : metaSoldier.armour.uniqueId);
            // items.RemoveItemFromInventory(input.itemId);
            // items.MoveItemToInventory(currentItem.uniqueId);
            // if (item is MetaWeapon) {
            //     metaSoldier.weapon = (MetaWeapon)item;
            // } else {
            //     metaSoldier.armour = (MetaArmour)item;
            // }
            // output.soldierId = input.soldierId;
            // output.weaponName = metaSoldier.weapon.name;
            // output.armourName = metaSoldier.armour.name;
            
            // presenter.Present(output);
        }
    }
}
