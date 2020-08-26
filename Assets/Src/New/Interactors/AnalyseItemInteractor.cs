using Data;
using Workers;

namespace Interactors {
    
    public class AnalyseItemInteractor : Interactor<AnalyseItemOutput> {

        public ISoldierStore soldierStore { private get; set; }

        public void Interact(AnalyseItemInput input) {
            var output = new AnalyseItemOutput();

            var items = metaGameState.metaItems;
            var item = items.Get(input.itemId);
            
            int cost;
            if (item is MetaArmour) {
                cost = soldierStore.GetArmourStats(item.name).cost;
            } else {
                cost = soldierStore.GetWeaponStats(item.name).cost;
            }
            items.Remove(item.uniqueId);
            items.AddBlueprint(item);
            metaGameState.credits.Deduct(cost);
            
            var itemsList = new WorkshopItemList(metaGameState);
            output.state = itemsList.GetList();
            
            presenter.Present(output);
        }
    }
}
