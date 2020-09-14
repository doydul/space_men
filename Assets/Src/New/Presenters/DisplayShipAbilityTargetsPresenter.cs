using Data;
using UnityEngine;

public class DisplayShipAbilityTargetsPresenter : Presenter, IPresenter<DisplayShipAbilityTargetsOutput> {
  
    public static DisplayShipAbilityTargetsPresenter instance { get; private set; }

    public Map map;
    public UIData uiData;
    public MapHighlighter mapHighlighter;
    public ShipAbilitiesPanel shipAbilitiesPanel;
    public MetaSoldierSelectPanel metaSoldierSelectPanel;
    
    void Awake() {
        instance = this;
    }
    
    public void Present(DisplayShipAbilityTargetsOutput input) {
        shipAbilitiesPanel.Close();
        if (input.possibleTargetMetaSoldiers != null) {
            metaSoldierSelectPanel.Open(input.possibleTargetMetaSoldiers, (selectedSoldier) => {
                uiData.selectedMetaSoldier = selectedSoldier;
                uiData.shipActions = input.possibleActions;
                DisplayMapHighlights(input.possibleActions);
            });
        } else {
            uiData.shipActions = input.possibleActions;
            DisplayMapHighlights(input.possibleActions);
        }
    }

    void DisplayMapHighlights(ShipAction[] possibleActions) {
        foreach (var shipAction in possibleActions) {
            mapHighlighter.HighlightTile(map.GetTileAt(new Vector2(shipAction.target.x, shipAction.target.y)), Color.yellow);
        }
    }
}

