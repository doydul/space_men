using System.Collections.Generic;
using System.Linq;

namespace DataTypes {

    public class MetaSoldiers {

        IDDictionary<MetaSoldier> metaSoldiers;
        MetaSoldier[] squad;

        public MetaSoldiers() {
            metaSoldiers = new IDDictionary<MetaSoldier>();
            squad = new MetaSoldier[6];
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

        public void UpdateSquadRoster(long soldierId, int squadIndex) {
            var metaSoldier = Get(soldierId);
            squad[squadIndex] = metaSoldier;
        }

        public MetaSoldier Get(long id) {
            return metaSoldiers.GetElement(id);
        }
        
        public IEnumerable<MetaSoldier> GetAll() {
            return metaSoldiers.GetElements();
        }

        public IEnumerable<MetaSoldier> GetSquad() {
            return new List<MetaSoldier>(squad);
        }

        public MetaSoldier GetAtSquadIndex(int index) {
            return squad[index];
        }

        public void Remove(long id) {
            metaSoldiers.RemoveElement(id);
            for (int i = 0; i < squad.Length; i++) {
                if (squad[i] != null && squad[i].uniqueId == id) squad[i] = null;
            }
        }
    }
}