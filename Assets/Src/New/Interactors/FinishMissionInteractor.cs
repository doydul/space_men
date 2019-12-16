using Data;
using Workers;

namespace Interactors {
    
    public class FinishMissionInteractor : Interactor<FinishMissionOutput> {

        public IMissionStore missionStore { private get; set; }
        public MetaGameState metaGameState { private get; set; }

        public void Interact(FinishMissionInput input) {
            var output = new FinishMissionOutput();
            
            var mission = missionStore.GetMission(gameState.campaign, gameState.mission);
            foreach (var reward in mission.rewards) {
                reward.Grant(metaGameState);
            }
            
            presenter.Present(output);
        }
    }
}
