using Interactors;
using Data;

public class ScriptingController : Controller {

    public FinishMissionInteractor finishMissionInteractor { get; set; }
    public CompleteSecondaryMissionInteractor completeSecondaryMissionInteractor { get; set; }
    public SpawnAliensInteractor spawnAliensInteractor { get; set; }
    
    public void FinishMission() {
        finishMissionInteractor.Interact(new FinishMissionInput());
    }

    public void CompleteSecondaryObjective(int missionIndex) {
        completeSecondaryMissionInteractor.Interact(new CompleteSecondaryMissionInput {
            index = missionIndex
        });
    }

    public void SpawnAliens(string alienType, int xPos, int yPos, Direction facing, int groupSize = 1) {
        spawnAliensInteractor.Interact(new SpawnAliensInput {
            alienType = alienType,
            xPos = xPos,
            yPos = yPos,
            groupSize = groupSize,
            facing = facing
        });
    }
}
