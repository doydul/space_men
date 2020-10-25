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
    public ExecuteShipAbilityInteractor executeShipAbilityInteractor { get; set; }
    public CollectAmmoInteractor collectAmmoInteractor { get; set; }
    [MakeObject] SoldierShootInteractor soldierShootInteractor;
    [MakeObject] ExecuteSpecialAbilityInteractor executeSpecialAbilityInteractor;
    
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

    public void PerformShipAction(Tile tile, ShipAbilityType abilityType) {
        if (disabled) return;
        long selectedSoldierId = uiData.selectedMetaSoldier?.soldierId ?? 0;
        var tilePos = tile.gridLocation;
        executeShipAbilityInteractor.Interact(new ExecuteShipAbilityInput {
            abilityType = abilityType,
            targetPosition = new Position((int)tilePos.x, (int)tilePos.y),
            metaSoldierId = selectedSoldierId
        });
    }

    public void CollectAmmo() {
        if (disabled) return;
        var soldierIndex = uiData.selectedActor.index;
        collectAmmoInteractor.Interact(new CollectAmmoInput {
            soldierIndex = soldierIndex
        });
    }

    public void PerformSpecialAction(Tile tile, SpecialAbilityType actionType) {
        if (disabled) return;
        executeSpecialAbilityInteractor.Interact(new ExecuteSpecialAbilityInput {
            soldierId = uiData.selectedActor.index,
            type = actionType,
            targetSquare = tile == null ? default(Position) : new Position((int)tile.gridLocation.x, (int)tile.gridLocation.y)
        });
    }
}
