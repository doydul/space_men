using Data;
using Workers;

namespace Interactors {
    
    public class ScrapItemInteractor : Interactor<ScrapItemOutput> {

        public ISoldierStore soldierStore { private get; set; }

        public void Interact(ScrapItemInput input) {
            var output = new ScrapItemOutput();

            var items = metaGameState.metaItems;
            var item = items.Get(input.itemId);

            int cost;
            if (item is MetaArmour) {
                cost = soldierStore.GetArmourStats(item.name).cost;
            } else {
                cost = soldierStore.GetWeaponStats(item.name).cost;
            }
            metaGameState.credits.Add(cost / 2);
            items.Remove(item.uniqueId);
            
            var itemsList = new WorkshopItemList(metaGameState);
            output.state = itemsList.GetList();
            
            presenter.Present(output);
        }
    }
}
