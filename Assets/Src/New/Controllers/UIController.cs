using Interactors;
using Data;
using UnityEngine.SceneManagement;

public class UIController : Controller {
    
    public InfoPanel infoPanel; // TODO refactor these to use pure presenters
    public UIData uiData;
    public BlackFade blackFade;

    public ProgressGamePhaseInteractor progressGamePhaseInteractor { get; set; }
    
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
        if (disabled) return;
        infoPanel.Display(message);
    }
}
