using System.Collections.Generic;

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
            var id = allMetaItems.AddElement(metaItem);
            metaItem.uniqueId = id;
            inventoryItems.Add(metaItem);
            return id;
        }

        public MetaItem Get(long id) {
            return allMetaItems.GetElement(id);
        }
        
        public IEnumerable<MetaItem> GetAll() {
            return allMetaItems.GetElements();
        }

        public IEnumerable<MetaItem> GetInventoryItems() {
            return new List<MetaItem>(inventoryItems);
        }

        public void Remove(long id) {
            var item = Get(id);
            allMetaItems.Remove(id);
            inventoryItems.Remove(item);
        }
    }
}