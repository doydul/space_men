using Data;
using Workers;

namespace Interactors {
    
    public class BuildItemInteractor : Interactor<BuildItemOutput> {

        public ISoldierStore soldierStore { private get; set; }

        public void Interact(BuildItemInput input) {
            var output = new BuildItemOutput();

            var items = metaGameState.metaItems;
            var blueprint = items.GetBlueprint(input.itemName);
            MetaItem newItem;
            int cost;
            if (blueprint is MetaArmour) {
                newItem = new MetaArmour { name = blueprint.name };
                cost = soldierStore.GetArmourStats(blueprint.name).cost;
            } else {
                newItem = new MetaWeapon { name = blueprint.name };
                cost = soldierStore.GetWeaponStats(blueprint.name).cost;
            }
            var id = items.Add(newItem);
            items.MoveItemToInventory(id);
            metaGameState.credits.Deduct(cost);
            
            var itemsList = new WorkshopItemList(metaGameState);
            output.state = itemsList.GetList();
            
            presenter.Present(output);
        }
    }
}
