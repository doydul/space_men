using System.Collections.Generic;

namespace DataTypes {

    public class MetaSoldiers {

        IDDictionary<MetaSoldier> metaSoldiers;

        public MetaSoldiers() {
            metaSoldiers = new IDDictionary<MetaSoldier>();
        }

        public long Add(MetaSoldier metaSoldier) {
            if (metaSoldier.uniqueId != 0) {
                metaSoldiers.AddElement(metaSoldier, metaSoldier.uniqueId);
                return metaSoldier.uniqueId;
            } else {
                var id = metaSoldiers.AddElement(metaSoldier);
                metaSoldier.uniqueId = id;
                return id;
            }
        }

        public MetaSoldier Get(long id) {
            return metaSoldiers.GetElement(id);
        }
        
        public IEnumerable<MetaSoldier> GetAll() {
            return metaSoldiers.GetElements();
        }

        public void Remove(long id) {
            metaSoldiers.RemoveElement(id);
        }
    }
}