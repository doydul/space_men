using System.Linq;
using Data;
using UnityEngine;

public class ActorActionsPresenter : Presenter, IPresenter<ActorActionsOutput> {

    public static ActorActionsPresenter instance { get; private set; }

    public UIData uiData;
    public MapHighlighter mapHighlighter;
    public GameObject turnButtonsContainer;
    
    void Awake() {
        instance = this;
    }
    
    public void Present(ActorActionsOutput input) {
        uiData.actorActions = input.actions;
        mapHighlighter.HighlightPossibleActions(input.actions);
        if (input.actions.Any(action => action.type == ActorActionType.Turn)) {
            turnButtonsContainer.SetActive(true);
        } else {
            turnButtonsContainer.SetActive(false);
        }
    }
}

