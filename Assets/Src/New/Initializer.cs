using UnityEngine;

using Interactors;
using Workers;

public class Initializer : MonoBehaviour {
    
    public UIController uiController;
    public MapController mapController;
    
    public SelectedItemInfoPresenter selectedItemInfoPresenter;
    public SoldierPossibleMovesPresenter soldierPossibleMovesPresenter;
    
    void Awake() {
        Storage.Init(new MissionStore());
        
        var inspectSelectedItemInteractor = new InspectSelectedItemInteractor();
        inspectSelectedItemInteractor.presenter = selectedItemInfoPresenter;
        uiController.inspectItemIntractor = inspectSelectedItemInteractor;
        
        var soldierPossibleMovesInteractor = new SoldierPossibleMovesInteractor();
        soldierPossibleMovesInteractor.presenter = soldierPossibleMovesPresenter;
        mapController.soldierPossibleMovesInteractor = soldierPossibleMovesInteractor;
    }
}
