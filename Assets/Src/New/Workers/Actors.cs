using System.Collections.Generic;
using DataTypes;

namespace Workers {
    
    public class Actors {

        IDDictionary<Data.Actor> actors;
        
        public Actors() {
            actors = new IDDictionary<Data.Actor>();
        }

        public long AddActor(Data.Actor actor) {
            var uniqueId = actors.AddElement(actor);
            actor.uniqueId = uniqueId;
            return uniqueId;
        }

        public Data.Actor GetActor(long id) {
            return actors.GetElement(id);
        }
        
        public IEnumerable<Data.Actor> GetActors() {
            return actors.GetElements();
        }

        public void RemoveActor(long id) {
            actors.RemoveElement(id);
        }
    }
}
