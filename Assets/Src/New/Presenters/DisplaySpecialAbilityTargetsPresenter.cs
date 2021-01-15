using Data;
using UnityEngine;

public class DisplaySpecialAbilityTargetsPresenter : Presenter, IPresenter<DisplaySpecialAbilityTargetsOutput> {
  
    public static DisplaySpecialAbilityTargetsPresenter instance { get; private set; }

    public Map map;
    public UIData uiData;
    public MapController mapController;
    public MapHighlighter mapHighlighter;
    
    void Awake() {
        instance = this;
    }
    
    public void Present(DisplaySpecialAbilityTargetsOutput input) {
        if (input.executeImmediately) {
            mapController.PerformSpecialAction(null, input.type);
            return;
        }
        uiData.actorActions = input.possibleActions;
        DisplayMapHighlights(input.possibleActions);
    }

    void DisplayMapHighlights(ActorAction[] possibleActions) {
        Debug.Log("Number of possible actions: " + possibleActions.Length);
        foreach (var action in possibleActions) {
            mapHighlighter.HighlightTile(map.GetTileAt(new Vector2(action.target.x, action.target.y)), Color.red);
        }
    }
}

