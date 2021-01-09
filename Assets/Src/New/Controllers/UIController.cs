using Interactors;
using Data;
using UnityEngine.SceneManagement;

public class UIController : Controller {
    
    public InfoPanel infoPanel; // TODO refactor these to use pure presenters
    public UIData uiData;
    public BlackFade blackFade;

    public ProgressGamePhaseInteractor progressGamePhaseInteractor { get; set; }
    public OpenShipAbilitiesInteractor openShipAbilitiesInteractor { get; set; }
    public DisplayShipAbilityTargetsInteractor displayShipAbilityTargetsInteractor { get; set; }
    [MakeObject] DisplaySpecialAbilityTargetsInteractor displaySpecialAbilityTargetsInteractor;
    
    public void ProgressGamePhase() {
        if (!disabled) progressGamePhaseInteractor.Interact(new ProgressGamePhaseInput());
    }
    
    public void ShowSelectedActorInfo() {
        if (!disabled) infoPanel.Display(uiData.selectedActor);
    }

    public void CloseInfoPanel() {
        if (!disabled) infoPanel.Close();
    }

    public void ContinueToRewardOverview() {
        if (!disabled) {
            blackFade.BeginFade(() => {
                SceneManager.LoadScene("MissionCompleteView");
            });
        }
    }

    public void ShowMessagePopup(string message) {
        infoPanel.Display(message);
    }

    public void OpenShipAbilitiesPanel() {
        if (disabled) return;
        openShipAbilitiesInteractor.Interact(new OpenShipAbilitiesInput {});
    }

    public void DisplayShipAbilityTargets(ShipAbilityType abilityType) {
        if (disabled) return;
        displayShipAbilityTargetsInteractor.Interact(new DisplayShipAbilityTargetsInput {
            abilityType = abilityType
        });
    }

    public void DisplaySpecialAbilityTargets(SpecialAbilityType actionType) {
        if (disabled) return;
        displaySpecialAbilityTargetsInteractor.Interact(new DisplaySpecialAbilityTargetsInput {
            soldierId = uiData.selectedActor.index,
            type = actionType
        });
    }
}
