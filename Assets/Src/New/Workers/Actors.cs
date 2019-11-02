using System.Collections.Generic;

namespace Workers {
    
    public class Actors {

        Dictionary<long, Data.Actor> actors;
        
        public Actors() {
            actors = new Dictionary<long, Data.Actor>();
        }

        public long AddActor(Data.Actor actor) {
            var id = GenerateUniqueId();
            actors.Add(id, actor);
            actor.uniqueId = id;
            return id;
        }

        public Data.Actor GetActor(long id) {
            return actors[id];
        }
        
        public IEnumerable<Data.Actor> GetActors() {
            return new List<Data.Actor>(actors.Values);
        }

        public void RemoveActor(long id) {
            actors.Remove(id);
        }

        long GenerateUniqueId() {
            return System.DateTime.Now.ToFileTime() + UnityEngine.Random.Range(0, 1000);
        }
    }
}
