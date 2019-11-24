using Interactors;
using Data;

public class ScriptingController : Controller {

    public FinishMissionInteractor finishMissionInteractor { get; set; }
    
    public void FinishMission() {
        finishMissionInteractor.Interact(new FinishMissionInput());
    }
}
