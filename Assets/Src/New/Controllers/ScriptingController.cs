using Interactors;
using Data;

public class ScriptingController : Controller {

    public FinishMissionInteractor finishMissionInteractor { get; set; }
    public CompleteSecondaryMissionInteractor completeSecondaryMissionInteractor { get; set; }
    
    public void FinishMission() {
        finishMissionInteractor.Interact(new FinishMissionInput());
    }

    public void CompleteSecondaryObjective(int missionIndex) {
        completeSecondaryMissionInteractor.Interact(new CompleteSecondaryMissionInput {
            index = missionIndex
        });
    }
}
