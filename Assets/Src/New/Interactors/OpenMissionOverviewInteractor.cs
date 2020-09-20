using Data;
using Workers;

namespace Interactors {
    
    public class OpenMissionOverviewInteractor : Interactor<OpenMissionOverviewOutput> {

        [Dependency] IMissionStore missionStore;

        public void Interact(OpenMissionOverviewInput input) {
            var output = new OpenMissionOverviewOutput();
            
            var mission = missionStore.GetMission(metaGameState.currentCampaign, metaGameState.currentMission);
            output.missionBriefingText = mission.briefing;
            
            presenter.Present(output);
        }
    }
}
