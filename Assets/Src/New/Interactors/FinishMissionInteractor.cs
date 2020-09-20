using System.Linq;
using System;
using Data;
using Workers;

namespace Interactors {
    
    public class FinishMissionInteractor : Interactor<FinishMissionOutput> {

        [Dependency] GameState gameState;
        [Dependency] IMissionStore missionStore;
        public ICampaignStore campaignStore { private get; set; }

        public void Interact(FinishMissionInput input) {
            var output = new FinishMissionOutput();

            output.missionName = metaGameState.currentMission;
            output.campaignName = metaGameState.currentCampaign;
            
            var mission = missionStore.GetMission(gameState.campaign, gameState.mission);
            foreach (var reward in mission.rewards) {
                reward.Grant(metaGameState);
            }

            output.completedSecondaryObjectIds = Enumerable.Range(0, mission.secondaryMissions.Length).Where(index => gameState.IsSecondaryObjectiveComplete(index)).ToArray();
            var campaign = campaignStore.GetCampaign(metaGameState.currentCampaign);
            var currentMissionIndex = Array.IndexOf(campaign.missionNames, metaGameState.currentMission);
            if (currentMissionIndex + 1 >= campaign.missionNames.Length) {
                output.campaignFinished = true;
            } else {
                metaGameState.currentMission = campaign.missionNames[currentMissionIndex + 1];
            }
            
            presenter.Present(output);
        }
    }
}
