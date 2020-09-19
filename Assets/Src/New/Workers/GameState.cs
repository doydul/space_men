using System.Collections.Generic;
using DataTypes;

namespace Workers {
    
    public class GameState {

        public IMapStore mapStore { private get; set; }

        public MapState map { get; private set; }
        
        Actors actors;
        HashSet<int> completedSecondaryObjectives;

        public Data.GamePhase currentPhase { get; private set; }
        public int threatTimer { get; private set; }
        public int currentThreatLevel { get; private set; }
        public string campaign { get; private set; }
        public string mission { get; private set; }
        public ShipEnergy shipEnergy { get; private set; }

        public void Init() {
            actors = new Actors();
            completedSecondaryObjectives = new HashSet<int>();
            shipEnergy = new ShipEnergy();

            map = new MapState();
            map.Init(mapStore.GetMap());
            this.campaign = MetaGameState.instance.currentCampaign;
            this.mission = MetaGameState.instance.currentMission;
        }

        public Data.Actor GetActor(long uniqueId) {
            return actors.GetActor(uniqueId);
        }
        
        public IEnumerable<Data.Actor> GetActors() {
            return actors.GetActors();
        }
        
        public long AddActor(Data.Actor actor, bool background = false) {
            var cell = map.GetCell(actor.position);
            if (background) {
                cell.backgroundActor = actor;
            } else {
                cell.actor = actor;
            }
            return actors.AddActor(actor);
        }

        public void RemoveActor(long actorId) {
            var actor = GetActor(actorId);
            var cell = map.GetCell(actor.position);
            cell.RemoveActor(actorId);
            actors.RemoveActor(actorId);
        }

        public void SetCurrentPhase(Data.GamePhase gamePhase) {
            currentPhase = gamePhase;
        }

        public void MarkSecondaryObjectiveComplete(int index) {
            completedSecondaryObjectives.Add(index);
        }

        public bool IsSecondaryObjectiveComplete(int index) {
            return completedSecondaryObjectives.Contains(index);
        }

        public void IncrementThreatTimer() {
            threatTimer++;
        }

        public void IncrementThreatLevel() {
            threatTimer = 0;
            currentThreatLevel++;
        }
    }
}
