using Interactors;
using Data;

public class MapController : Controller {
    
    public static MapController instance { get; private set; }
    
    void Awake() {
        instance = this;
    }
    
    public SoldierPossibleMovesInteractor soldierPossibleMovesInteractor { private get; set; }
    public MissionStartInteractor missionStartInteractor { private get; set; }
    public MoveSoldierInteractor moveSoldierInteractor { private get; set; }
    
    public void ShowPossibleMovesFor(long soldierIndex) {
        var input = new SoldierPossibleMovesInput {
            soldierIndex = soldierIndex
        };
        soldierPossibleMovesInteractor.Interact(input);
    }
    
    public void StartMission() {
        missionStartInteractor.Interact(new MissionStartInput());
    }
    
    public void MoveSoldier(long soldierIndex, UnityEngine.Vector2 targetLocation) {
        moveSoldierInteractor.Interact(new MoveSoldierInput {
            soldierIndex = soldierIndex,
            targetPosition = new Position(
                (int)targetLocation.x,
                (int)targetLocation.y
            ) 
        });
    }
}
