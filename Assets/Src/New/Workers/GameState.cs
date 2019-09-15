using System.Collections.Generic;

namespace Workers {
    
    public class GameState {

        public IMapStore mapStore { private get; set; }

        public MapState map { get; private set; }
        
        Actors actors;

        public void Init() {
            actors = new Actors();

            map = new MapState();
            map.Init(mapStore.GetMap());
        }

        public Data.Actor GetActor(long uniqueId) {
            return actors.GetActor(uniqueId);
        }
        
        public IEnumerable<Data.Actor> GetActors() {
            return actors.GetActors();
        }
        
        public long AddActor(Data.Actor actor) {
            return actors.AddActor(actor);
        }
    }
}
