using Interactors;
using Data;

public class MissionOverviewController : Controller {

    public OpenMissionOverviewInteractor openMissionOverviewInteractor { get; set; }
    public LoadMissionInteractor loadMissionInteractor { get; set; }

    public void InitializeMissionOverview() {
        if (!disabled) {
            openMissionOverviewInteractor.Interact(new OpenMissionOverviewInput());
        }
    }

    public void StartMission() {
        if (!disabled) {
            loadMissionInteractor.Interact(new LoadMissionInput());
        }
    }
}
