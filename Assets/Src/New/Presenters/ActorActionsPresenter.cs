using System.Linq;
using Data;
using UnityEngine;

public class ActorActionsPresenter : Presenter, IPresenter<ActorActionsOutput> {

    public static ActorActionsPresenter instance { get; private set; }

    public UIData uiData;
    public Map map;
    public MapHighlighter mapHighlighter;
    public TurnButtons turnButtons;
    public WorldButton collectAmmoButton;
    public SpecialAbilityPanel abilityPanel;
    
    void Awake() {
        instance = this;
    }
    
    public void Present(ActorActionsOutput input) {
        uiData.actorActions = input.actions;
        // mapHighlighter.HighlightPossibleActions(input.actions);
        if (input.actions.Any(action => action.type == ActorActionType.Turn)) {
            turnButtons.Show();
        } else {
            turnButtons.Hide();
        }
        abilityPanel.Open(input.actions);
    }
}

