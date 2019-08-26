using Interactors;
using Data;

public class MapController : Controller {
    
    public static MapController instance { get; private set; }
    
    void Awake() {
        instance = this;
    }
    
    public SoldierPossibleMovesInteractor soldierPossibleMovesInteractor { private get; set; }
    
    public void ShowPossibleMovesFor(int soldierIndex) {
        var input = new SoldierPossibleMovesInput {
            soldierIndex = soldierIndex
        };
        soldierPossibleMovesInteractor.Interact(input);
    }
}
