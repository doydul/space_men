namespace DataTypes {

    public class MetaSoldiers {

        IDDictionary<MetaSoldier> metaSoldiers;

        public MetaSoldiers() {
            metaSoldiers = new IDDictionary<MetaSoldier>();
        }

        public long Add(MetaSoldier metaSoldier) {
            var id = metaSoldiers.AddElement(metaSoldier);
            metaSoldier.uniqueId = id;
            return id;
        }

        public MetaSoldier Get(long id) {
            return metaSoldiers.GetElement(id);
        }
        
        public IEnumerable<MetaSoldier> GetAll() {
            return metaSoldiers.GetElements();
        }

        public void Remove(long id) {
            metaSoldiers.Remove(id);
        }
    }
}