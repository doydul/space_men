using System.Collections.Generic;

namespace Workers {
    
    public class GameState {

        public IMapStore mapStore { private get; set; }

        public MapState map { get; private set; }
        
        Actors actors;

        public Data.GamePhase currentPhase { get; private set; }
        public string campaign { get; private set; }
        public string mission { get; private set; }

        public void Init(string campaignName, string missionName) {
            actors = new Actors();

            map = new MapState();
            map.Init(mapStore.GetMap());
            this.campaign = campaignName;
            this.mission = missionName;
        }

        public Data.Actor GetActor(long uniqueId) {
            return actors.GetActor(uniqueId);
        }
        
        public IEnumerable<Data.Actor> GetActors() {
            return actors.GetActors();
        }
        
        public long AddActor(Data.Actor actor) {
            var cell = map.GetCell(actor.position);
            cell.actor = actor;
            return actors.AddActor(actor);
        }

        public void RemoveActor(long actorId) {
            var actor = GetActor(actorId);
            var cell = map.GetCell(actor.position);
            cell.ClearActor();
            actors.RemoveActor(actorId);
        }

        public void SetCurrentPhase(Data.GamePhase gamePhase) {
            currentPhase = gamePhase;
        }
    }
}
