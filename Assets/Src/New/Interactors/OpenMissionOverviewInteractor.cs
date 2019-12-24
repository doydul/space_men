using Data;
using Workers;

namespace Interactors {
    
    public class OpenMissionOverviewInteractor : Interactor<OpenMissionOverviewOutput> {

        public IMissionStore missionStore { private get; set; }
        public MetaGameState metaGameState { private get; set; }

        public void Interact(OpenMissionOverviewInput input) {
            var output = new OpenMissionOverviewOutput();
            
            var mission = missionStore.GetMission(metaGameState.currentCampaign, metaGameState.currentMission);
            output.missionBriefingText = mission.briefing;
            
            presenter.Present(output);
        }
    }
}
