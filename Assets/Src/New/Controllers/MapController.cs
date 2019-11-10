using Interactors;
using Data;

public class MapController : Controller {
    
    public static MapController instance { get; private set; }
    
    public UIData uiData;

    void Awake() {
        instance = this;
    }
    
    public MissionStartInteractor missionStartInteractor { private get; set; }
    public MoveSoldierInteractor moveSoldierInteractor { private get; set; }
    public ActorActionsInteractor actorActionsInteractor { private get; set; }
    public TurnSoldierInteractor turnSoldierInteractor { private get; set; }
    public SoldierShootInteractor soldierShootInteractor { get; set; }
    
    public void StartMission() {
        missionStartInteractor.Interact(new MissionStartInput());
    }
    
    public void MoveSoldier(long soldierIndex, UnityEngine.Vector2 targetLocation) {
        if (disabled) return;
        moveSoldierInteractor.Interact(new MoveSoldierInput {
            soldierIndex = soldierIndex,
            targetPosition = new Position(
                (int)targetLocation.x,
                (int)targetLocation.y
            ) 
        });
    }

    public void DisplayActions(long actorIndex) {
        if (disabled) return;
        actorActionsInteractor.Interact(new ActorActionsInput {
            index = actorIndex
        });
    }

    public void SoldierShoot(long soldierIndex, long targetIndex) {
        if (disabled) return;
        soldierShootInteractor.Interact(new SoldierShootInput {
            index = soldierIndex,
            targetIndex = targetIndex
        });
    }

    public void TurnSoldierLeft() {
        if (disabled) return;
        turnSoldierInteractor.Interact(new TurnSoldierInput {
            index = uiData.selectedTile.GetActor<Soldier>().index,
            newFacing = Direction.Left
        });
    }

    public void TurnSoldierRight() {
        if (disabled) return;
        turnSoldierInteractor.Interact(new TurnSoldierInput {
            index = uiData.selectedTile.GetActor<Soldier>().index,
            newFacing = Direction.Right
        });
    }

    public void TurnSoldierUp() {
        if (disabled) return;
        turnSoldierInteractor.Interact(new TurnSoldierInput {
            index = uiData.selectedTile.GetActor<Soldier>().index,
            newFacing = Direction.Up
        });
    }

    public void TurnSoldierDown() {
        if (disabled) return;
        turnSoldierInteractor.Interact(new TurnSoldierInput {
            index = uiData.selectedTile.GetActor<Soldier>().index,
            newFacing = Direction.Down
        });
    }
}
