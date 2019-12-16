using Data;
using Workers;

namespace Interactors {
    
    public class CompleteSecondaryMissionInteractor : Interactor<CompleteSecondaryMissionOutput> {
        
        public MetaGameState metaGameState { private get; set;  }
        public IMissionStore missionStore { private get; set; }

        public void Interact(CompleteSecondaryMissionInput input) {
            if (gameState.IsSecondaryObjectiveComplete(input.index)) return;

            var output = new CompleteSecondaryMissionOutput();
            
            var mission = missionStore.GetMission(gameState.campaign, gameState.mission);
            var secondaryMission = mission.secondaryMissions[input.index];
            foreach (var reward in secondaryMission.rewards) {
                reward.Grant(metaGameState);
            }
            gameState.MarkSecondaryObjectiveComplete(input.index);
            
            presenter.Present(output);
        }
    }
}
