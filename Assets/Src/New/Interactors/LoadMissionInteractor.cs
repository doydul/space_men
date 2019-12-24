using Data;
using Workers;

namespace Interactors {
    
    public class LoadMissionInteractor : Interactor<LoadMissionOutput> {

        public MetaGameState metaGameState { private get; set; }

        public void Interact(LoadMissionInput input) {
            var output = new LoadMissionOutput();
            
            output.campaignName = metaGameState.currentCampaign;
            output.missionName = metaGameState.currentMission;
            
            presenter.Present(output);
        }
    }
}
