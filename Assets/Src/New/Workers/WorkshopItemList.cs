using System.Linq;
using Data;

namespace Workers {

    public class WorkshopItemList {
        MetaGameState metaGameState;

        public WorkshopItemList(MetaGameState metaGameState) {
            this.metaGameState = metaGameState;
        }

        public WorkshopState GetList() {
            var state = new WorkshopState();
            state.items = metaGameState.metaItems.GetInventoryItems().Select(item => ConvertMetaItem(item)).ToArray();
            state.blueprints = metaGameState.metaItems.GetBlueprints().Select(item => ConvertMetaItem(item)).ToArray();
            return state;
        }

        WorkshopItem ConvertMetaItem(MetaItem metaItem) {
            return new WorkshopItem {
                itemId = metaItem.uniqueId,
                itemName = metaItem.name,
                type = metaItem is MetaWeapon ? WorkshopItemType.Weapon : WorkshopItemType.Armour
            };
        }
    }
}