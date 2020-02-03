using System.Collections.Generic;
using System.Linq;

namespace DataTypes {

    public class MetaItems {

        IDDictionary<MetaItem> allMetaItems;
        List<MetaItem> inventoryItems;
        List<MetaItem> blueprints;

        public MetaItems() {
            allMetaItems = new IDDictionary<MetaItem>();
            inventoryItems = new List<MetaItem>();
            blueprints = new List<MetaItem>();
        }

        public long Add(MetaItem metaItem) {
            if (metaItem.uniqueId != 0) {
                allMetaItems.AddElement(metaItem, metaItem.uniqueId);
                return metaItem.uniqueId;
            } else {
                var id = allMetaItems.AddElement(metaItem);
                metaItem.uniqueId = id;
                return id;
            }
        }

        public void AddBlueprint(MetaItem metaItem) {
            blueprints.Add(metaItem);
        }

        public void MoveItemToInventory(long itemId) {
            if (!inventoryItems.Any(item => item.uniqueId == itemId)) {
                inventoryItems.Add(allMetaItems.GetElement(itemId));
            }
        }

        public void RemoveItemFromInventory(long itemId) {
            inventoryItems.Remove(allMetaItems.GetElement(itemId));
        }

        public MetaItem Get(long id) {
            return allMetaItems.GetElement(id);
        }
        
        public IEnumerable<MetaItem> GetAll() {
            return allMetaItems.GetElements();
        }

        public IEnumerable<MetaItem> GetBlueprints() {
            return new List<MetaItem>(blueprints);
        }

        public IEnumerable<MetaItem> GetInventoryItems() {
            return new List<MetaItem>(inventoryItems);
        }

        public void Remove(long id) {
            var item = Get(id);
            allMetaItems.RemoveElement(id);
            inventoryItems.Remove(item);
        }
    }
}